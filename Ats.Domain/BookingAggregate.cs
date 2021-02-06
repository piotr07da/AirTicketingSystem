using Ats.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain
{
    public class BookingAggregate : IChangable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private Guid _id;
        private Guid _flightInstanceId;
        private Guid _customerId;
        private bool _isCanceled;

        public BookingAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public Guid Id => _id;

        public void Start(Guid bookingId, Guid flightInstanceId)
        {
            if (_id != Guid.Empty) throw new DomainLogicException($"This booking is already started and has id {_id}.");

            _aggregateEventApplier.ApplyNewEvent(new BookingStartedEvent(bookingId, flightInstanceId));
        }

        public void Cancel()
        {
            EnsureIsCreated();

            _aggregateEventApplier.ApplyNewEvent(new BookingCanceledEvent(_id));
        }

        public void Confirm()
        {
            EnsureIsCreated();

            if (_customerId.IsUndefined())
            {
                throw new DomainLogicException($"Cannot confirm incomplete booking. Customer is undefined.");
            }
        }

        private void EnsureIsCreated()
        {
            if (_id.IsUndefined())
            {
                throw new DomainLogicException($"This booking is not created yet.");
            }
        }

        private void Apply(BookingStartedEvent e)
        {
            _id = e.BookingId;
            _flightInstanceId = e.FlightInstanceId;
        }

        private void Apply(BookingCanceledEvent e)
        {
            _isCanceled = true;
        }
    }
}
