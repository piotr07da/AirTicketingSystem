using Ats.Core.Commands;
using System;

namespace Ats.Application.FlightInstance
{
    public class CreateFlightInstanceCommand : ICommand
    {
        public CreateFlightInstanceCommand(Guid flightInstanceId, Guid flightUid, int flightVersion, DateTime departureDate)
        {
            FlightInstanceId = flightInstanceId;
            FlightUid = flightUid;
            FlightVersion = flightVersion;
            DepartureDate = departureDate;
        }

        public Guid FlightInstanceId { get; set; }
        public Guid FlightUid { get; set; }
        public int FlightVersion { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
