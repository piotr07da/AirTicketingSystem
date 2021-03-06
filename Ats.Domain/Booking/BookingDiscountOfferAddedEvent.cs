﻿using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingDiscountOfferAddedEvent : IEvent
    {
        public BookingDiscountOfferAddedEvent(Guid bookingId, string offerName, decimal offerValue)
        {
            BookingId = bookingId;
            OfferName = offerName;
            OfferValue = offerValue;
        }

        public Guid BookingId { get; }
        public string OfferName { get; }
        public decimal OfferValue { get; }
    }
}
