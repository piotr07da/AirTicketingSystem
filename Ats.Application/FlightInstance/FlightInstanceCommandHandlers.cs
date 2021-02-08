using Ats.Core.Commands;
using Ats.Core.Domain;
using Ats.Domain.Flight;
using Ats.Domain.FlightInstance;
using System;
using System.Threading.Tasks;

namespace Ats.Application.FlightInstance
{
    public class FlightInstanceCommandHandlers :
        ICommandHandler<CreateFlightInstanceCommand>
    {
        private readonly FlightInstanceCreationService _flightInstanceCreationService;
        private readonly IRepository<FlightInstanceAggregate> _flightInstanceRepository;
        private readonly IRepository<FlightAggregate> _flightRepository;

        public FlightInstanceCommandHandlers(
            FlightInstanceCreationService flightInstanceCreationService,
            IRepository<FlightInstanceAggregate> flightInstanceRepository,
            IRepository<FlightAggregate> flightRepository)
        {
            _flightInstanceCreationService = flightInstanceCreationService ?? throw new ArgumentNullException(nameof(flightInstanceCreationService));
            _flightInstanceRepository = flightInstanceRepository ?? throw new ArgumentNullException(nameof(flightInstanceRepository));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }

        public async Task HandleAsync(CreateFlightInstanceCommand command)
        {
            var flight = await _flightRepository.GetAsync(command.FlightUid);
            var flightInstance = await _flightInstanceRepository.GetAsync(command.FlightInstanceId);

            _flightInstanceCreationService.CreateFlightInstance(flight, flightInstance, command.FlightInstanceId, new FlightInstancePrice(command.Price), command.DepartureDate);

            await _flightRepository.SaveAsync(command.FlightUid, flight, command.FlightVersion);
            await _flightInstanceRepository.SaveAsync(command.FlightInstanceId, flightInstance, 0);
        }
    }
}
