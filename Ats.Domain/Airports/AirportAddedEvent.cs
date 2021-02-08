using Ats.Core.Domain;
using System;

namespace Ats.Domain.Airports
{
    public class AirportAddedEvent : IEvent
    {
        public AirportAddedEvent(Guid airportsId, string airportCode, string airportContinent)
        {
            AirportsId = airportsId;
            AirportCode = airportCode;
            AirportContinent = airportContinent;
        }

        public Guid AirportsId { get; set; }
        public string AirportCode { get; set; }
        public string AirportContinent { get; set; }
    }
}
