using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingPriceChangedEvent : IEvent
    {
        public BookingPriceChangedEvent(Guid bookingId, decimal bookingPrice)
        {
            BookingId = bookingId;
            BookingPrice = bookingPrice;
        }

        public Guid BookingId { get; }
        public decimal BookingPrice { get; }
    }
}
