using Ats.Core.Commands;
using System;

namespace Ats.Application.FlightInstance
{
    public class CreateFlightInstanceCommand : ICommand
    {
        public CreateFlightInstanceCommand(Guid flightInstanceId, Guid flightUid, int flightVersion, decimal price, DateTime departureDate)
        {
            FlightInstanceId = flightInstanceId;
            FlightUid = flightUid;
            FlightVersion = flightVersion;
            Price = price;
            DepartureDate = departureDate;
        }

        public Guid FlightInstanceId { get; }
        public Guid FlightUid { get; }
        public int FlightVersion { get; }
        public decimal Price { get; }
        public DateTime DepartureDate { get; }
    }
}
