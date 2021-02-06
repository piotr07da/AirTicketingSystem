using Ats.Application;
using Ats.Domain;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Booking
{
    public class given_FlightInstance_with_no_Bookings
    {
        private Guid _flightInstanceId;

        [SetUp]
        public void given()
        {
            _flightInstanceId = Guid.NewGuid();
        }

        [Test]
        public async Task when_StartBooking_then_BookingStarted_and_BookingAdded_to_FlightInstance()
        {
            var bookingId = Guid.NewGuid();

            await Tester.TestAsync(gwt => gwt
                .Given(_flightInstanceId, new FlightInstanceCreatedEvent(_flightInstanceId))
                .When(new StartBookingCommand(bookingId, _flightInstanceId, 1))
                .Then(_flightInstanceId, new BookingAddedEvent(_flightInstanceId, bookingId))
                .Then(bookingId, new BookingStartedEvent(bookingId, _flightInstanceId))
                );
        }
    }
}
