using Ats.Core.Domain;
using System;

namespace Ats.Domain
{
    public class BookingCanceledEvent : IEvent
    {
        public BookingCanceledEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }

        public Guid BookingId { get; set; }
    }
}
