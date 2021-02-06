using Ats.Core.Domain;
using Ats.Domain.Booking;
using System;
using System.Collections.Generic;

namespace Ats.Domain.FlightInstance
{
    public class FlightInstanceAggregate : IChangable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private FlightInstanceId _id;
        private DateTime _departureDate;
        private HashSet<Guid> _bookings = new HashSet<Guid>();

        public FlightInstanceAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public FlightInstanceId Id => _id;
        public DateTime DepartureDate => _departureDate;

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
            _departureDate = e.DepartureDate;
        }

        private void Apply(FlightInstanceBookingAddedEvent e)
        {
            _bookings.Add(e.BookingId);
        }
    }
}
