using Ats.Application.Flight;
using Ats.Domain;
using Ats.Domain.Flight;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Flight
{
    public class given_not_scheduled_Flight
    {
        private readonly Guid _flightUid = Guid.NewGuid();
        private readonly string _flightId = "LOT 99999 XYZ";
        private readonly string _departureAirport = "BZG";
        private readonly string _arrivalAirport = "KUL";
        private readonly DayOfWeek[] _daysOfWeek = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
        private readonly TimeSpan _departureHour = new TimeSpan(6, 0, 0);

        private ScheduleFlightCommand _scheduleFlightCommand;

        [SetUp]
        public void SetUp()
        {
            _scheduleFlightCommand = new ScheduleFlightCommand(_flightUid, _flightId, _departureAirport, _arrivalAirport, _daysOfWeek, _departureHour);
        }

        [Test]
        public async Task when_ScheduleFlight_then_flight_scheduled()
        {
            await Tester.TestAsync(gwt => gwt
                .When(_scheduleFlightCommand)
                .Then(_flightUid, new FlightScheduledEvent(_flightUid, _flightId, _departureAirport, _arrivalAirport, _daysOfWeek, _departureHour))
            );
        }

        [Test]
        public async Task when_ScheduleFlight_with_flight_id_without_airlines_designator_then_excpetion_thrown()
        {
            _scheduleFlightCommand.FlightId = "99999 XYZ";

            await Tester.TestAsync(gwt => gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("incorrect")
            );
        }

        [Test]
        public async Task when_ScheduleFlight_with_duplicated_days_of_week_then_exception_thrown()
        {
            _scheduleFlightCommand.DaysOfWeek = new[] { DayOfWeek.Tuesday, DayOfWeek.Tuesday };

            await Tester.TestAsync(gwt => gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("duplicated")
            );
        }

        [Test]
        public async Task when_ScheduleFlight_without_days_of_week_then_exception_thrown()
        {
            _scheduleFlightCommand.DaysOfWeek = new DayOfWeek[0];

            await Tester.TestAsync(gwt => gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("no departure days")
            );
        }
    }
}
