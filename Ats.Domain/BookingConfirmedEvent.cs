using Ats.Core.Domain;
using System;

namespace Ats.Domain
{
    public class BookingConfirmedEvent : IEvent
    {
        public BookingConfirmedEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }

        public Guid BookingId { get; set; }
    }
}
