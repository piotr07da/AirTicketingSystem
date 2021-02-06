using Ats.Core.Commands;
using System;

namespace Ats.Application
{
    public class StartBookingCommand : ICommand
    {
        public StartBookingCommand(Guid bookingId, Guid flightInstanceId, int flightInstanceVersion)
        {
            BookingId = bookingId;
            FlightInstanceId = flightInstanceId;
            FlightInstanceVersion = flightInstanceVersion;
        }

        public Guid BookingId { get; set; }
        public Guid FlightInstanceId { get; set; }
        public int FlightInstanceVersion { get; set; }
    }
}
