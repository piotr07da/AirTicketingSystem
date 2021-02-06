using Ats.Core.Domain;
using System;

namespace Ats.Domain
{
    public class BookingAddedEvent : IEvent
    {
        public BookingAddedEvent(Guid flightInstanceId, Guid bookingId)
        {
            FlightInstanceId = flightInstanceId;
            BookingId = bookingId;
        }

        public Guid FlightInstanceId;
        public Guid BookingId { get; set; }
    }
}
