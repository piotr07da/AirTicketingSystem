﻿using Ats.Application.Flight;
using Ats.Domain;
using Ats.Domain.Airports;
using Ats.Domain.Flight;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Flight
{
    public class given_some_airports_and_not_scheduled_Flight
    {
        private GivenWhenThen _gwt;

        private Guid _flightUid = Guid.NewGuid();
        private string _flightId = "LOT 99999 XYZ";
        private Airport _departureAirport = new Airport("BZG", Continent.Europe);
        private Airport _arrivalAirport = new Airport("KUL", Continent.Asia);
        private DayOfWeek[] _daysOfWeek = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
        private TimeSpan _departureHour = new TimeSpan(6, 0, 0);
        private ScheduleFlightCommand _scheduleFlightCommand;

        [SetUp]
        public void SetUp()
        {
            _scheduleFlightCommand = new ScheduleFlightCommand(_flightUid, _flightId, _departureAirport.Code, _arrivalAirport.Code, _daysOfWeek, _departureHour);

            _gwt = new GivenWhenThen()
                .Given(GlobalAirportsId.Id,
                    new AirportAddedEvent(GlobalAirportsId.Id, _departureAirport.Code, _departureAirport.Continent),
                    new AirportAddedEvent(GlobalAirportsId.Id, _arrivalAirport.Code, _arrivalAirport.Continent));
        }

        [Test]
        public async Task when_ScheduleFlight_then_flight_scheduled()
        {
            await Tester.TestAsync(_gwt
                .When(_scheduleFlightCommand)
                .Then(_flightUid, new FlightScheduledEvent(_flightUid, _flightId, _departureAirport.Code, _arrivalAirport.Code, _daysOfWeek, _departureHour))
            );
        }

        [Test]
        public async Task when_ScheduleFlight_with_not_existing_departure_Airport_then_excpetion_thrown()
        {
            _scheduleFlightCommand.DepartureAirport = "JFK";

            await Tester.TestAsync(_gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("does not exist")
            );
        }

        [Test]
        public async Task when_ScheduleFlight_with_not_existing_arrival_Airport_then_excpetion_thrown()
        {
            _scheduleFlightCommand.ArrivalAirport = "DXB";

            await Tester.TestAsync(_gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("does not exist")
            );
        }

        [Test]
        public async Task when_ScheduleFlight_with_flight_id_without_airlines_designator_then_excpetion_thrown()
        {
            _scheduleFlightCommand.FlightId = "99999 XYZ";

            await Tester.TestAsync(_gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("incorrect")
            );
        }

        [Test]
        public async Task when_ScheduleFlight_with_duplicated_days_of_week_then_exception_thrown()
        {
            _scheduleFlightCommand.DaysOfWeek = new[] { DayOfWeek.Tuesday, DayOfWeek.Tuesday };

            await Tester.TestAsync(_gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("duplicated")
            );
        }

        [Test]
        public async Task when_ScheduleFlight_without_days_of_week_then_exception_thrown()
        {
            _scheduleFlightCommand.DaysOfWeek = new DayOfWeek[0];

            await Tester.TestAsync(_gwt
                .When(_scheduleFlightCommand)
                .Throws<DomainLogicException>("no departure days")
            );
        }
    }
}