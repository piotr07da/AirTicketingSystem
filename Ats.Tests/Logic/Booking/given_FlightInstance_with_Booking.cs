using Ats.Application.Booking;
using Ats.Domain;
using Ats.Domain.FlightInstance;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Booking
{
    public class given_FlightInstance_with_Booking
    {
        private Guid _flightInstanceId;

        [SetUp]
        public void given()
        {
            _flightInstanceId = Guid.NewGuid();
        }

        [Test]
        public async Task when_StartBooking_for_Booking_that_already_exists_in_FlightInstance_then_exception_is_thrown()
        {
            var bookingId = Guid.NewGuid();

            await Tester.TestAsync(gwt => gwt
                .Given(_flightInstanceId, new FlightInstanceCreatedEvent(_flightInstanceId, Guid.NewGuid(), DateTime.Now), new FlightInstanceBookingAddedEvent(_flightInstanceId, bookingId))
                .When(new StartBookingCommand(bookingId, _flightInstanceId, 2))
                .Throws<DomainLogicException>("already")
                );
        }
    }
}
