using Ats.Application.FlightInstance;
using Ats.Domain;
using Ats.Domain.Flight;
using Ats.Domain.FlightInstance;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.FlightInstance
{
    public class given_scheduled_Flight_with_no_FlightInstances
    {
        private GivenWhenThen _gwt;

        private FlightUid _flightUid;

        [SetUp]
        public void given()
        {
            _flightUid = Guid.NewGuid();

            _gwt = new GivenWhenThen()
                .Given(_flightUid, new FlightScheduledEvent(_flightUid, "KLM 12345 BCA", "BZG", "KUL", new[] { DayOfWeek.Tuesday, DayOfWeek.Sunday }, new TimeSpan(12, 45, 0)));
        }

        [Test]
        public async Task when_CreateFlightInstance_then_FlightInstance_created_and_added_to_scheduled_Flight()
        {
            var flightInstanceId = Guid.NewGuid();
            var departureDate = new DateTime(2021, 2, 7);

            await Tester.TestAsync(_gwt
                .When(new CreateFlightInstanceCommand(flightInstanceId, _flightUid, 1, departureDate))
                .Then(_flightUid, new FlightInstanceAddedEvent(_flightUid, flightInstanceId))
                .Then(flightInstanceId, new FlightInstanceCreatedEvent(flightInstanceId, _flightUid, departureDate))
            );
        }

        [Test]
        public async Task when_CreateFlightInstance_with_departure_date_in_different_day_of_week_than_scheduled_for_flight_then_exception_thrown()
        {
            var flightInstanceId = Guid.NewGuid();
            var departureDate = new DateTime(2021, 2, 8); // this is Monday but flight is scheduled for Tuesday and Sunday (check given() method)

            await Tester.TestAsync(_gwt
                .When(new CreateFlightInstanceCommand(flightInstanceId, _flightUid, 1, departureDate))
                .Throws<DomainLogicException>("incorrect day of week")
            );
        }
    }
}
