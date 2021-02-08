using Ats.Core.Commands;
using Ats.Core.Domain;
using Ats.Domain.Airports;
using Ats.Domain.Flight;
using System;
using System.Threading.Tasks;

namespace Ats.Application.Flight
{
    public class FlightCommandHandlers :
        ICommandHandler<ScheduleFlightCommand>
    {
        private readonly FlightSchedulingService _flightSchedulingService;
        private readonly IRepository<FlightAggregate> _flightRepository;
        private readonly IRepository<AirportsAggregate> _airportsRepository;

        public FlightCommandHandlers(
            FlightSchedulingService flightSchedulingService,
            IRepository<FlightAggregate> flightRepository,
            IRepository<AirportsAggregate> airportsRepository)
        {
            _flightSchedulingService = flightSchedulingService ?? throw new ArgumentNullException(nameof(flightSchedulingService));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _airportsRepository = airportsRepository ?? throw new ArgumentNullException(nameof(airportsRepository));
        }

        public async Task HandleAsync(ScheduleFlightCommand command)
        {
            var flight = await _flightRepository.GetAsync(command.FlightUid);
            var airports = await _airportsRepository.GetAsync(GlobalAirportsId.Id);

            _flightSchedulingService.ScheduleFlight(flight, airports, command.FlightUid, command.FlightId, command.DepartureAirport, command.ArrivalAirport, command.DaysOfWeek, command.DepartureHour);

            await _flightRepository.SaveAsync(command.FlightUid, flight, 0);
        }
    }
}
