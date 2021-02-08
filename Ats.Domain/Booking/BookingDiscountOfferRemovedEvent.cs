using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingDiscountOfferRemovedEvent : IEvent
    {
        public BookingDiscountOfferRemovedEvent(Guid bookingId, string offerName)
        {
            BookingId = bookingId;
            OfferName = offerName;
        }

        public Guid BookingId { get; }
        public string OfferName { get; }
    }
}
