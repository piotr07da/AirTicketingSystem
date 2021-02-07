using Ats.Core.Domain;
using System;

namespace Ats.Domain.FlightInstance
{
    public class FlightInstanceCreatedEvent : IEvent
    {
        public FlightInstanceCreatedEvent(Guid flightInstanceId, Guid flightUid, DateTime departureDate)
        {
            FlightInstanceId = flightInstanceId;
            FlightUid = flightUid;
            DepartureDate = departureDate;
        }

        public Guid FlightInstanceId { get; set; }
        public Guid FlightUid { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
