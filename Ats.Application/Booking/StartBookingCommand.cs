using Ats.Core.Commands;
using System;

namespace Ats.Application.Booking
{
    public class StartBookingCommand : ICommand
    {
        public StartBookingCommand(Guid bookingId, Guid flightInstanceId, int flightInstanceVersion)
        {
            BookingId = bookingId;
            FlightInstanceId = flightInstanceId;
            FlightInstanceVersion = flightInstanceVersion;
        }

        public Guid BookingId { get; }
        public Guid FlightInstanceId { get; }
        public int FlightInstanceVersion { get; }
    }
}
