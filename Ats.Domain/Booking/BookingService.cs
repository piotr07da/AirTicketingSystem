using Ats.Domain.FlightInstance;

namespace Ats.Domain.Booking
{
    public class BookingService
    {
        public void StartBooking(FlightInstanceAggregate flightInstance, BookingAggregate booking, BookingId bookingId)
        {
            flightInstance.AddBooking(bookingId);
            booking.Start(bookingId, flightInstance.Id);
        }
    }
}
