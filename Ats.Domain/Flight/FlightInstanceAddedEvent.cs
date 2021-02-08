using Ats.Core.Domain;
using System;

namespace Ats.Domain.Flight
{
    public class FlightInstanceAddedEvent : IEvent
    {
        public FlightInstanceAddedEvent(Guid flightUid, Guid flightInstanceId)
        {
            FlightUid = flightUid;
            FlightInstanceId = flightInstanceId;
        }

        public Guid FlightUid { get; }
        public Guid FlightInstanceId { get; }
    }
}
