using Ats.Core.Commands;
using System;

namespace Ats.Application.Booking
{
    public class RefreshDiscountOffersCommand : ICommand
    {
        public RefreshDiscountOffersCommand(Guid bookingId, int bookingVersion)
        {
            BookingId = bookingId;
            BookingVersion = bookingVersion;
        }

        public Guid BookingId { get; set; }
        public int BookingVersion { get; set; }
    }
}
