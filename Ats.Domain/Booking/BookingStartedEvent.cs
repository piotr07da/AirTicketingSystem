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

        public Guid BookingId { get; }
        public Guid FlightInstanceId { get; }
        public decimal BookingPrice { get; }
    }
}
