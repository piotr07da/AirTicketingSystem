using Ats.Application.Booking;
using Ats.Domain;
using Ats.Domain.Booking;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Booking
{
    public class given_just_started_Booking
    {
        private Guid _bookingId;
        private GivenWhenThen _gwt;

        [SetUp]
        public void given()
        {
            _bookingId = Guid.NewGuid();

            _gwt = new GivenWhenThen()
                .Given(_bookingId, new BookingStartedEvent(_bookingId, Guid.NewGuid(), 50.00m));

        }

        [Test]
        public async Task when_CancelBooking_then_Booking_canceled()
        {
            await Tester.TestAsync(_gwt
                .When(new CancelBookingCommand(_bookingId, 1))
                .Then(_bookingId, new BookingCanceledEvent(_bookingId))
                );
        }

        [Test]
        public async Task when_ConfirmBooking_then_exception_thrown()
        {
            await Tester.TestAsync(_gwt
                .When(new ConfirmBookingCommand(_bookingId, 1))
                .Throws<DomainLogicException>("cannot confirm incomplete booking")
                );
        }
    }
}
