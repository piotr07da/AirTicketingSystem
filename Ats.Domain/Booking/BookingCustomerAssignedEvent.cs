using Ats.Core.Domain;
using System;

namespace Ats.Domain.Booking
{
    public class BookingCustomerAssignedEvent : IEvent
    {
        public BookingCustomerAssignedEvent(Guid bookingId, Guid customerId)
        {
            BookingId = bookingId;
            CustomerId = customerId;
        }

        public Guid BookingId { get; }
        public Guid CustomerId { get; }
    }
}
