﻿using Ats.Core.Commands;
using System;

namespace Ats.Application.Booking
{
    public class ApplyBookingDiscountCommand : ICommand
    {
        public ApplyBookingDiscountCommand(Guid bookingId, int bookingVersion, string discountOfferName)
        {
            BookingId = bookingId;
            BookingVersion = bookingVersion;
            DiscountOfferName = discountOfferName;
        }

        public Guid BookingId { get; set; }
        public int BookingVersion { get; set; }
        public string DiscountOfferName { get; set; }
    }
}
