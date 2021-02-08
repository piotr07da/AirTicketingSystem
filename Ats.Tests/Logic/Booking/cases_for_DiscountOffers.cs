using Ats.Application.Booking;
using Ats.Core.Domain;
using Ats.Domain.Airports;
using Ats.Domain.Booking;
using Ats.Domain.Customer;
using Ats.Domain.Flight;
using Ats.Domain.FlightInstance;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Booking
{
    public class cases_for_DiscountOffers
    {
        private Guid _bookingId = Guid.NewGuid();

        [Test]
        public async Task given_FlightInstance_departure_day_and_Customer_birhday_the_same_when_RefreshDiscountOffers_then_BirthdayDiscountOffer_added()
        {
            var customerId = Guid.NewGuid();
            var flightInstanceId = Guid.NewGuid();

            await Tester.TestAsync(gwt => gwt
                .Given(customerId, new CustomerRegisteredEvent(customerId, "Piotr", "Bejger", new DateTime(1905, 2, 6)))
                .Given(flightInstanceId, new FlightInstanceCreatedEvent(flightInstanceId, Guid.NewGuid(), 100.00m, new DateTime(2021, 2, 6)))
                .Given(_bookingId, new BookingStartedEvent(_bookingId, flightInstanceId, 100.00m), new BookingCustomerAssignedEvent(_bookingId, customerId))
                .When(new RefreshDiscountOffersCommand(_bookingId, 2))
                .Then(_bookingId, new BookingDiscountOfferAddedEvent(_bookingId, "BirthdayDiscount", 5.00m)) // Powinno być ThenContains - patrz TODO w klasie GivenWhenThen
            );
        }

        [Test]
        public async Task given_thursday_Flight_to_Africa_when_RefreshDiscountOffers_then_AfriceThursdayDiscountOffer_added()
        {
            var flightUid = Guid.NewGuid();
            var flightInstanceId = Guid.NewGuid();

            await Tester.TestAsync(gwt => gwt
                .Given(GlobalAirportsId.Id, new AirportAddedEvent(GlobalAirportsId.Id, "BZG", "Europe"), new AirportAddedEvent(GlobalAirportsId.Id, "MGQ", "Africa"))
                .Given(flightUid, new FlightScheduledEvent(flightUid, "KLM 12345 BCA", "BZG", "MGQ", new[] { DayOfWeek.Thursday }, new TimeSpan(10, 30, 0)))
                .Given(flightInstanceId, new FlightInstanceCreatedEvent(flightInstanceId, flightUid, 100.00m, new DateTime(2021, 2, 11)))
                .Given(_bookingId, new BookingStartedEvent(_bookingId, flightInstanceId, 100.00m))
                .When(new RefreshDiscountOffersCommand(_bookingId, 1))
                .Then(_bookingId, new BookingDiscountOfferAddedEvent(_bookingId, "AfricaThursdayDiscount", 5.00m)) // Powinno być ThenContains - patrz TODO w klasie GivenWhenThen
            );
        }

        [Test]
        public async Task given_thursday_Flight_somewhere_else_than_Africa_when_RefreshDiscountOffers_then_no_AfricaThursdayDiscountOffer_added()
        {
            var flightUid = Guid.NewGuid();
            var flightInstanceId = Guid.NewGuid();

            await Tester.TestAsync(gwt => gwt
                .Given(GlobalAirportsId.Id, new AirportAddedEvent(GlobalAirportsId.Id, "BZG", "Europe"), new AirportAddedEvent(GlobalAirportsId.Id, "KUL", "Asia"))
                .Given(flightUid, new FlightScheduledEvent(flightUid, "KLM 12345 BCA", "BZG", "KUL", new[] { DayOfWeek.Thursday }, new TimeSpan(10, 30, 0)))
                .Given(flightInstanceId, new FlightInstanceCreatedEvent(flightInstanceId, flightUid, 100.00m, new DateTime(2021, 2, 11)))
                .Given(_bookingId, new BookingStartedEvent(_bookingId, flightInstanceId, 100.00m))
                .When(new RefreshDiscountOffersCommand(_bookingId, 1))
                .Then(_bookingId, new IEvent[0]) // Powinno być ThenNot (wraz z podanymi eventem BookingDiscountOfferAddedEvent, który nie powinien wystąpić, to nie znaczy, że nie mogą wystąpić inne discounty) - patrz TODO w klasie GivenWhenThen
            );
        }
    }
}
