using Ats.Core.Commands;
using System;

namespace Ats.Application.Flight
{
    public class ScheduleFlightCommand : ICommand
    {
        public ScheduleFlightCommand(Guid flightUid, string flightId, string departureAirport, string arrivalAirport, DayOfWeek[] daysOfWeek, TimeSpan departureHour)
        {
            FlightUid = flightUid;
            FlightId = flightId;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            DaysOfWeek = daysOfWeek;
            DepartureHour = departureHour;
        }

        public Guid FlightUid { get; }
        public string FlightId { get; }
        public string DepartureAirport { get; }
        public string ArrivalAirport { get; }
        public DayOfWeek[] DaysOfWeek { get; }
        public TimeSpan DepartureHour { get; }
    }
}
