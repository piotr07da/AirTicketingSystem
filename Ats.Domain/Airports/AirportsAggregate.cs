using Ats.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ats.Domain.Airports
{
    public class AirportsAggregate : IChangeable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private AirportsId _id;
        private IList<Airport> _airports = new List<Airport>();

        public AirportsAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));

            _id = GlobalAirportsId.Id;
        }

        public Changes Changes { get; } = new Changes();

        public AirportsId Id => _id;
        public Airport[] Airports => _airports.ToArray();

        public void AddAirport(Airport airport)
        {
            _aggregateEventApplier.ApplyNewEvent(new AirportAddedEvent(_id, airport.Code, airport.Continent));
        }

        private void Apply(AirportAddedEvent e)
        {
            _airports.Add(new Airport(e.AirportCode, e.AirportContinent));
        }
    }
}
