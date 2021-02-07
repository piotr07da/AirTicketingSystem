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

        public Guid FlightUid { get; set; }
        public string FlightId { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DayOfWeek[] DaysOfWeek { get; set; }
        public TimeSpan DepartureHour { get; set; }
    }
}
