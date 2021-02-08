using Ats.Core.Commands;
using System;

namespace Ats.Application.Booking
{
    public class CancelBookingCommand : ICommand
    {
        public CancelBookingCommand(Guid bookingId, int bookingVersion)
        {
            BookingId = bookingId;
            BookingVersion = bookingVersion;
        }

        public Guid BookingId { get; }
        public int BookingVersion { get; }
    }
}
