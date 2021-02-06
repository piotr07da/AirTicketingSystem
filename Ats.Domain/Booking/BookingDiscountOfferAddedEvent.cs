using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingDiscountOfferAddedEvent : IEvent
    {
        public BookingDiscountOfferAddedEvent(Guid bookingId, string offerName, decimal offerValue)
        {
            BookingId = bookingId;
            OfferName = offerName;
            OfferValue = offerValue;
        }

        public Guid BookingId { get; set; }
        public string OfferName { get; set; }
        public decimal OfferValue { get; set; }
    }
}
