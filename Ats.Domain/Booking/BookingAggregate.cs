using Ats.Core.Domain;
using Ats.Domain.FlightInstance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ats.Domain.Booking
{
    public class BookingAggregate : IChangeable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;
        private readonly ITenant _tenant;

        private BookingId _id;
        private FlightInstanceId _flightInstanceId;
        private CustomerId _customerId;
        private decimal _price;
        private IDictionary<string, DiscountOffer> _discountOffers = new Dictionary<string, DiscountOffer>();
        private HashSet<string> _appliedDiscounts = new HashSet<string>();
        private bool _isCanceled;
        private bool _isConfirmed;

        public BookingAggregate(IAggregateEventApplier aggregateEventApplier, ITenant tenant)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
            _tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
        }

        public Changes Changes { get; } = new Changes();

        public BookingId Id => _id;
        public FlightInstanceId FlightInstanceId => _flightInstanceId;
        public CustomerId CustomerId => _customerId;
        public IDictionary<string, DiscountOffer> DiscountOffers => new Dictionary<string, DiscountOffer>(_discountOffers);

        public void Start(BookingId bookingId, FlightInstanceId flightInstanceId, FlightInstancePrice flightPrice)
        {
            if (_id.IsDefined) throw new DomainLogicException($"This booking is already started and has id {_id}.");

            _aggregateEventApplier.ApplyNewEvent(new BookingStartedEvent(bookingId, flightInstanceId, flightPrice.Value));
        }

        public void AddDiscountOffer(DiscountOffer discountOffer)
        {
            EnsureIsCreated();
            EnsureIsNotCanceled();
            EnsureIsNotConfirmed();

            if (_discountOffers.ContainsKey(discountOffer.Name))
            {
                throw new DomainLogicException($"Cannot add {discountOffer.Name} discount offer. This discount offer already exists.");
            }

            _aggregateEventApplier.ApplyNewEvent(new BookingDiscountOfferAddedEvent(_id, discountOffer.Name, discountOffer.Value));
        }

        public void RemoveDiscountOffer(string discountOfferName)
        {
            EnsureIsCreated();
            EnsureIsNotCanceled();
            EnsureIsNotConfirmed();

            if (!_discountOffers.ContainsKey(discountOfferName))
            {
                throw new DomainLogicException($"Cannot remove {discountOfferName} discount offer. This discount offer does not exist for this booking.");
            }

            _aggregateEventApplier.ApplyNewEvent(new BookingDiscountOfferRemovedEvent(_id, discountOfferName));
        }

        public void ApplyDiscountOffer(string discountOfferName)
        {
            EnsureIsCreated();
            EnsureIsNotCanceled();
            EnsureIsNotConfirmed();

            if (!_discountOffers.TryGetValue(discountOfferName, out DiscountOffer discountOffer))
            {
                throw new DomainLogicException($"{discountOfferName} discount is not available for this booking. Available discount offers are [{string.Join(", ", _discountOffers.Values.Select(d => d.Name))}].");
            }

            if (_appliedDiscounts.Contains(discountOffer.Name))
            {
                throw new DomainLogicException($"{discountOffer.Name} has been already applied.");
            }

            var price = new BookingPrice(_price);
            price.Decrease(discountOffer.Value);

            _aggregateEventApplier.ApplyNewEvent(new BookingPriceChangedEvent(_id, price.Value));
            if (_tenant.Group == TenantGroup.A)
            {
                _aggregateEventApplier.ApplyNewEvent(new BookingDiscountAppliedEvent(_id, discountOffer.Name, discountOffer.Value));
            }
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

        private void EnsureIsNotCanceled()
        {
            if (_isCanceled)
            {
                throw new DomainLogicException($"This booking has been canceled.");
            }
        }

        private void EnsureIsNotConfirmed()
        {
            if (_isConfirmed)
            {
                throw new DomainLogicException($"This booking has been confirmed.");
            }
        }

        private void Apply(BookingStartedEvent e)
        {
            _id = e.BookingId;
            _flightInstanceId = e.FlightInstanceId;
            _price = e.BookingPrice;
        }

        private void Apply(BookingCustomerAssignedEvent e)
        {
            _customerId = e.CustomerId;
        }

        private void Apply(BookingDiscountOfferAddedEvent e)
        {
            _discountOffers.Add(e.OfferName, new DiscountOffer(e.OfferName, e.OfferValue));
        }

        private void Apply(BookingDiscountOfferRemovedEvent e)
        {
            _discountOffers.Remove(e.OfferName);
        }

        private void Apply(BookingPriceChangedEvent e)
        {
            _price = e.BookingPrice;
        }

        private void Apply(BookingDiscountAppliedEvent e)
        {
            _appliedDiscounts.Add(e.DiscountOfferName);
        }

        private void Apply(BookingCanceledEvent e)
        {
            _isCanceled = true;
        }
    }
}
