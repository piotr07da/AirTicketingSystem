using Ats.Application.Booking;
using Ats.Domain;
using Ats.Domain.Booking;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Booking
{
    public class given_tenant_A_and_Booking_with_discount_offers
    {
        private GivenWhenThen _gwt;

        private Guid _bookingId = Guid.NewGuid();

        [SetUp]
        public void given()
        {
            FakeTenant.SetTenantGroup(TenantGroup.A);

            _gwt = new GivenWhenThen()
                .Given(_bookingId,
                    new BookingStartedEvent(_bookingId, Guid.NewGuid(), 100.00m),
                    new BookingDiscountOfferAddedEvent(_bookingId, "Offer1", 10.00m),
                    new BookingDiscountOfferAddedEvent(_bookingId, "Offer2", 95.00m)
                );
        }

        [Test]
        public async Task when_ApplyBookingDiscount_then_Booking_price_decreased_by_discount_offer_value_and_discount_applied()
        {
            await Tester.TestAsync(_gwt
                .When(new ApplyBookingDiscountCommand(_bookingId, 3, "Offer1"))
                .ThenIdentical(_bookingId,
                    new BookingPriceChangedEvent(_bookingId, 90.00m),
                    new BookingDiscountAppliedEvent(_bookingId, "Offer1", 10.00m)
                )
            );
        }

        [Test]
        public async Task when_ApplyBookingDiscount_with_discount_value_exceeding_possible_discount_then_Booking_price_decreased_to_its_minimal_value_of_20_euro_and_discount_applied()
        {
            await Tester.TestAsync(_gwt
                .When(new ApplyBookingDiscountCommand(_bookingId, 3, "Offer2"))
                .ThenIdentical(_bookingId,
                    new BookingPriceChangedEvent(_bookingId, 20.00m),
                    new BookingDiscountAppliedEvent(_bookingId, "Offer2", 95.00m)
                )
            );
        }

        [Test]
        public async Task and_given_booking_discount_applied_when_ApplyBookingDiscount_for_same_discount_offer_then_exception_thrown()
        {
            await Tester.TestAsync(_gwt
                .Given(_bookingId,
                    new BookingPriceChangedEvent(_bookingId, 90.00m),
                    new BookingDiscountAppliedEvent(_bookingId, "Offer1", 10.00m))
                .When(new ApplyBookingDiscountCommand(_bookingId, 5, "Offer1"))
                .Throws<DomainLogicException>("already applied")
            );
        }

        [Test]
        public async Task when_ApplyBookingDiscount_that_is_not_available_for_Booking_then_exception_thrown()
        {
            await Tester.TestAsync(_gwt
                .When(new ApplyBookingDiscountCommand(_bookingId, 3, "Offer3"))
                .Throws<DomainLogicException>("not available")
            );
        }
    }
}
