using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingDiscountAppliedEvent : IEvent
    {
        public BookingDiscountAppliedEvent(Guid bookingId, string discountOfferName, decimal discountValue)
        {
            BookingId = bookingId;
            DiscountOfferName = discountOfferName;
            DiscountValue = discountValue;
        }

        public Guid BookingId { get; }
        public string DiscountOfferName { get; }
        public decimal DiscountValue { get; }
    }
}
