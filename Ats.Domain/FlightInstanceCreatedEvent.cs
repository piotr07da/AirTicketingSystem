using Ats.Core.Domain;
using System;

namespace Ats.Domain
{
    public class FlightInstanceCreatedEvent : IEvent
    {
        public FlightInstanceCreatedEvent(Guid flightInstanceId)
        {
            FlightInstanceId = flightInstanceId;
        }

        public Guid FlightInstanceId { get; set; }
    }
}
