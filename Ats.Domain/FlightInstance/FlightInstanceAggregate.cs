using Ats.Core.Domain;
using Ats.Domain.Booking;
using Ats.Domain.Flight;
using System;
using System.Collections.Generic;

namespace Ats.Domain.FlightInstance
{
    public class FlightInstanceAggregate : IChangeable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private FlightInstanceId _id;
        private FlightUid _flightUid;
        private FlightInstancePrice _price;
        private DateTime _departureDate;
        private HashSet<Guid> _bookings = new HashSet<Guid>();

        public FlightInstanceAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public FlightInstanceId Id => _id;
        public FlightUid FlightUid => _flightUid;
        public FlightInstancePrice Price => _price;
        public DateTime DepartureDate => _departureDate;


        public void Create(FlightInstanceId id, FlightUid flightUid, FlightInstancePrice price, DateTime departureDate)
        {
            _aggregateEventApplier.ApplyNewEvent(new FlightInstanceCreatedEvent(id, flightUid, price.Value, departureDate));
        }

        public void AddBooking(BookingId bookingId)
        {
            EnsureIsCreated();

            if (_bookings.Contains(bookingId))
            {
                throw new DomainLogicException($"This flight instance {_id} already has this booking {bookingId}.");
            }

            _aggregateEventApplier.ApplyNewEvent(new FlightInstanceBookingAddedEvent(_id, bookingId));
        }

        private void EnsureIsCreated()
        {
            if (_id.IsUndefined)
            {
                throw new DomainLogicException($"This flight isntance is not created yet.");
            }
        }

        private void Apply(FlightInstanceCreatedEvent e)
        {
            _id = e.FlightInstanceId;
            _flightUid = e.FlightUid;
            _price = new FlightInstancePrice(e.Price);
            _departureDate = e.DepartureDate;
        }

        private void Apply(FlightInstanceBookingAddedEvent e)
        {
            _bookings.Add(e.BookingId);
        }
    }
}
