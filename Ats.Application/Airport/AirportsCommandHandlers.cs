﻿using Ats.Core.Commands;
using Ats.Core.Domain;
using Ats.Domain.Airports;
using System;
using System.Threading.Tasks;

namespace Ats.Application.Airports
{
    public class AirportsCommandHandlers :
        ICommandHandler<AddAirportCommand>
    {
        private readonly IRepository<AirportsAggregate> _airportsRepository;

        public AirportsCommandHandlers(
            IRepository<AirportsAggregate> airportsRepository)
        {
            _airportsRepository = airportsRepository ?? throw new ArgumentNullException(nameof(airportsRepository));
        }

        public async Task HandleAsync(AddAirportCommand command)
        {
            var airports = await _airportsRepository.GetAsync(command.AirportsId);

            airports.AddAirport(new Airport(command.AirportCode, command.AirportContinent));

            await _airportsRepository.SaveAsync(command.AirportsId, airports, command.AirportsVersion);
        }
    }
}
