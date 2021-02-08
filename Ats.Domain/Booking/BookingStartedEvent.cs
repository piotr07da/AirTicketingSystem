﻿using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingStartedEvent : IEvent
    {
        public BookingStartedEvent(Guid bookingId, Guid flightInstanceId, decimal bookingPrice)
        {
            BookingId = bookingId;
            FlightInstanceId = flightInstanceId;
            BookingPrice = bookingPrice;
        }

        public Guid BookingId { get; set; }
        public Guid FlightInstanceId { get; set; }
        public decimal BookingPrice { get; set; }
    }
}
