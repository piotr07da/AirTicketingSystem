using Ats.Core.Domain;
using Ats.Domain.Airports;
using Ats.Domain.FlightInstance;
using System;
using System.Collections.Generic;

namespace Ats.Domain.Flight
{
    public class FlightAggregate : IChangable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private FlightUid _uid;
        private FlightId _flightId;
        private AirportCode _departureAirport;
        private AirportCode _arrivalAirport;
        private DayOfWeek[] _daysOfWeek;

        public FlightAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public FlightUid Uid => _uid;
        public AirportCode DepartureAirport => _departureAirport;
        public AirportCode ArrivalAirport => _arrivalAirport;
        public DayOfWeek[] DaysOfWeek => _daysOfWeek;

        public void Schedule(FlightUid uid, FlightId flightId, AirportCode departureAirport, AirportCode arrivalAirport, DayOfWeek[] daysOfWeek, DayTime departureHour)
        {
            if (_uid.IsDefined)
            {
                throw new DomainLogicException($"Cannot schedule this flight. This flight is already scheduled with unique id: {_uid} and id: {_flightId}.");
            }

            if (daysOfWeek.Length == 0)
            {
                throw new DomainLogicException($"There are no departure days defined for this flight.");
            }

            if (DaysAreNotUnique(daysOfWeek))
            {
                throw new DomainLogicException($"Days of week for flight are duplicated. Each day has to be unique (be listed only once).");
            }

            _aggregateEventApplier.ApplyNewEvent(new FlightScheduledEvent(uid, flightId, departureAirport, arrivalAirport, daysOfWeek, departureHour));
        }

        public void AddFlightInstance(FlightInstanceId flightInstaceId)
        {
            _aggregateEventApplier.ApplyNewEvent(new FlightInstanceAddedEvent(_uid, flightInstaceId));
        }

        private bool DaysAreNotUnique(DayOfWeek[] daysOfWeek)
        {
            var hs = new HashSet<DayOfWeek>();
            foreach (var d in daysOfWeek)
            {
                if (hs.Contains(d))
                {
                    return true;
                }
                hs.Add(d);
            }
            return false;
        }

        private void EnsureIsCreated()
        {
            if (_uid.IsUndefined)
            {
                throw new DomainLogicException($"This flight is not created yet.");
            }
        }

        private void Apply(FlightScheduledEvent e)
        {
            _uid = e.FlightUid;
            _flightId = e.FlightId;
            _departureAirport = e.DepartureAirport;
            _arrivalAirport = e.ArrivalAirport;
            _daysOfWeek = e.DaysOfWeek;
        }

        private void Apply(FlightInstanceAddedEvent e)
        {

        }
    }
}
