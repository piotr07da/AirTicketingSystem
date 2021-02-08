using Ats.Core.Domain;
using System;

namespace Ats.Domain.FlightInstance
{
    public class FlightInstanceBookingAddedEvent : IEvent
    {
        public FlightInstanceBookingAddedEvent(Guid flightInstanceId, Guid bookingId)
        {
            FlightInstanceId = flightInstanceId;
            BookingId = bookingId;
        }

        public Guid FlightInstanceId;
        public Guid BookingId { get; }
    }
}
