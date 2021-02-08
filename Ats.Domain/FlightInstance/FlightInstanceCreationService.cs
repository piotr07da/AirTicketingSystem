using Ats.Domain.Flight;
using System;
using System.Linq;

namespace Ats.Domain.FlightInstance
{
    public class FlightInstanceCreationService
    {
        public void CreateFlightInstance(FlightAggregate flight, FlightInstanceAggregate flightInstance, FlightInstanceId flightInstanceId, FlightInstancePrice price, DateTime departureDate)
        {
            if (!flight.DaysOfWeek.Contains(departureDate.DayOfWeek))
            {
                throw new DomainLogicException($"Departure date at incorrect day of week. Possible days of week for this flight are {string.Join(", ", flight.DaysOfWeek)}.");
            }

            flight.AddFlightInstance(flightInstanceId);
            flightInstance.Create(flightInstanceId, flight.Uid, price, departureDate);
        }
    }
}
