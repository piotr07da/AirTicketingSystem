using Ats.Core.Domain;
using System;

namespace Ats.Domain.FlightInstance
{
    public class FlightInstanceCreatedEvent : IEvent
    {
        public FlightInstanceCreatedEvent(Guid flightInstanceId, Guid flightId, DateTime departureDate)
        {
            FlightInstanceId = flightInstanceId;
            FlightId = flightId;
            DepartureDate = departureDate;
        }

        public Guid FlightInstanceId { get; set; }
        public Guid FlightId { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
