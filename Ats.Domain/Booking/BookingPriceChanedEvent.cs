using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingPriceChanedEvent : IEvent
    {
        public BookingPriceChanedEvent(Guid bookingId, decimal bookingPrice)
        {
            BookingId = bookingId;
            BookingPrice = bookingPrice;
        }

        public Guid BookingId { get; set; }
        public decimal BookingPrice { get; set; }
    }
}
