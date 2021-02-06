using Ats.Core.Commands;
using System;

namespace Ats.Application
{
    public class CancelBookingCommand : ICommand
    {
        public CancelBookingCommand(Guid bookingId, int bookingVersion)
        {
            BookingId = bookingId;
            BookingVersion = bookingVersion;
        }

        public Guid BookingId { get; set; }
        public int BookingVersion { get; set; }
    }
}
