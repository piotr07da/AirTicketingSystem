using Ats.Core.Domain;
using Ats.Domain.FlightInstance;
using System;
using System.Collections.Generic;

namespace Ats.Domain.Booking
{
    public class BookingAggregate : IChangable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private BookingId _id;
        private FlightInstanceId _flightInstanceId;
        private CustomerId _customerId;
        private IDictionary<string, DiscountOffer> _discountOffers = new Dictionary<string, DiscountOffer>();
        private bool _isCanceled;

        public BookingAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public BookingId Id => _id;
        public FlightInstanceId FlightInstanceId => _flightInstanceId;
        public CustomerId CustomerId => _customerId;

        public void Start(BookingId bookingId, FlightInstanceId flightInstanceId)
        {
            if (_id.IsDefined) throw new DomainLogicException($"This booking is already started and has id {_id}.");

            _aggregateEventApplier.ApplyNewEvent(new BookingStartedEvent(bookingId, flightInstanceId));
        }

        public void AddDiscountOffer(DiscountOffer discountOffer)
        {
            if (_discountOffers.ContainsKey(discountOffer.Name))
            {
                throw new DomainLogicException($"{discountOffer.Name} discount offer already exists.");
            }

            _aggregateEventApplier.ApplyNewEvent(new BookingDiscountOfferAddedEvent(_id, discountOffer.Name, discountOffer.Value));
        }

        public void Cancel()
        {
            EnsureIsCreated();

            _aggregateEventApplier.ApplyNewEvent(new BookingCanceledEvent(_id));
        }

        public void Confirm()
        {
            EnsureIsCreated();

            if (_customerId.IsUndefined)
            {
                throw new DomainLogicException($"Cannot confirm incomplete booking. Customer is undefined.");
            }
        }

        private void EnsureIsCreated()
        {
            if (_id.IsUndefined)
            {
                throw new DomainLogicException($"This booking is not created yet.");
            }
        }

        private void Apply(BookingStartedEvent e)
        {
            _id = e.BookingId;
            _flightInstanceId = e.FlightInstanceId;
        }

        private void Apply(BookingCustomerAssignedEvent e)
        {
            _customerId = e.CustomerId;
        }

        private void Apply(BookingDiscountOfferAddedEvent e)
        {
            _discountOffers.Add(e.OfferName, new DiscountOffer(e.OfferName, e.OfferValue));
        }

        private void Apply(BookingCanceledEvent e)
        {
            _isCanceled = true;
        }
    }
}
