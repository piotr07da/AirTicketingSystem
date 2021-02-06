using System;

namespace Ats.Domain
{
    public class BookingService
    {
        public void StartBooking(FlightInstanceAggregate flightInstance, BookingAggregate booking, Guid bookingId)
        {
            flightInstance.AddBooking(bookingId);
            booking.Start(bookingId, flightInstance.Id);
        }
    }
}
