using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingStartedEvent : IEvent
    {
        public BookingStartedEvent(Guid bookingId, Guid flightInstanceId)
        {
            BookingId = bookingId;
            FlightInstanceId = flightInstanceId;
        }

        public Guid BookingId { get; set; }
        public Guid FlightInstanceId { get; set; }
    }
}
