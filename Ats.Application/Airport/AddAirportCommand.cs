using Ats.Core.Commands;
using System;

namespace Ats.Application.Airports
{
    public class AddAirportCommand : ICommand
    {
        public AddAirportCommand(Guid airportsId, int airportsVersion, string airportCode, string airportContinent)
        {
            AirportsId = airportsId;
            AirportsVersion = airportsVersion;
            AirportCode = airportCode;
            AirportContinent = airportContinent;
        }

        public Guid AirportsId { get; set; }
        public int AirportsVersion { get; set; }
        public string AirportCode { get; set; }
        public string AirportContinent { get; set; }
    }
}
