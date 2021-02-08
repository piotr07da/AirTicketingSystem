using Ats.Domain.Airports;
using System;
using System.Linq;

namespace Ats.Domain.Flight
{
    public class FlightSchedulingService
    {
        public void ScheduleFlight(FlightAggregate flight, AirportsAggregate airports, FlightUid flightUid, FlightId flightId, AirportCode departureAirport, AirportCode arrivalAirport, DayOfWeek[] daysOfWeek, DayTime departureHour)
        {
            if (!airports.Airports.Any(a => a.Code == departureAirport))
            {
                throw new DomainLogicException($"Departure airport {departureAirport} does not exist.");
            }

            if (!airports.Airports.Any(a => a.Code == arrivalAirport))
            {
                throw new DomainLogicException($"Arrival airport {departureAirport} does not exist.");
            }

            flight.Schedule(flightUid, flightId, departureAirport, arrivalAirport, daysOfWeek, departureHour);
        }
    }
}
