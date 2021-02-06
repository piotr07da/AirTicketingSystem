using Ats.Application.Booking;
using Ats.Domain.Booking;
using Ats.Domain.Customer;
using Ats.Domain.FlightInstance;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Booking
{
    public class cases_for_DiscountOffers
    {
        [Test]
        public async Task given_FlightInstance_departure_day_and_Customer_birhday_the_same_when_RefreshDiscountOffers_then_BirthdayDiscountOffer_added()
        {
            var customerId = Guid.NewGuid();
            var flightInstanceId = Guid.NewGuid();
            var bookingId = Guid.NewGuid();

            await Tester.TestAsync(gwt => gwt
                .Given(customerId, new CustomerRegisteredEvent(customerId, "Piotr", "Bejger", new DateTime(1905, 2, 6)))
                .Given(flightInstanceId, new FlightInstanceCreatedEvent(flightInstanceId, Guid.NewGuid(), new DateTime(2020, 2, 6)))
                .Given(bookingId, new BookingStartedEvent(bookingId, flightInstanceId), new BookingCustomerAssignedEvent(bookingId, customerId))
                .When(new RefreshDiscountOffersCommand(bookingId, 2))
                .Then(bookingId, new BookingDiscountOfferAddedEvent(bookingId, "BirthdayDiscount", 5.00m))
            );
        }
    }
}
