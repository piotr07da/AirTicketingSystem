using Ats.Core.Domain;
using System;
using System.Collections.Generic;

namespace Ats.Domain
{
    public class FlightInstanceAggregate : IChangable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private Guid _id;
        private HashSet<Guid> _bookings = new HashSet<Guid>();

        public FlightInstanceAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public Guid Id => _id;

        public void AddBooking(Guid bookingId)
        {
            EnsureIsCreated();

            if (_bookings.Contains(bookingId))
            {
                throw new DomainLogicException($"This flight instance {_id} already has this booking {bookingId}.");
            }

            _aggregateEventApplier.ApplyNewEvent(new BookingAddedEvent(_id, bookingId));
        }

        private void EnsureIsCreated()
        {
            if (_id.IsUndefined())
            {
                throw new DomainLogicException($"This flight isntance is not created yet.");
            }
        }

        private void Apply(FlightInstanceCreatedEvent e)
        {
            _id = e.FlightInstanceId;
        }

        private void Apply(BookingAddedEvent e)
        {
            _bookings.Add(e.BookingId);
        }
    }
}
