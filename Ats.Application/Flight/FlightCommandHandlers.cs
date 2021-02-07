using Ats.Core.Commands;
using Ats.Core.Domain;
using Ats.Domain.Flight;
using System;
using System.Threading.Tasks;

namespace Ats.Application.Flight
{
    public class FlightCommandHandlers :
        ICommandHandler<ScheduleFlightCommand>
    {
        private readonly IRepository<FlightAggregate> _flightRepository;

        public FlightCommandHandlers(
            IRepository<FlightAggregate> flightRepository)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }

        public async Task HandleAsync(ScheduleFlightCommand command)
        {
            var flight = await _flightRepository.GetAsync(command.FlightUid);

            flight.Schedule(command.FlightUid, command.FlightId, command.DepartureAirport, command.ArrivalAirport, command.DaysOfWeek, command.DepartureHour);

            await _flightRepository.SaveAsync(command.FlightUid, flight, 0);
        }
    }
}
