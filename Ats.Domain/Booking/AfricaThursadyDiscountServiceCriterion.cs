using Ats.Core.Domain;
using Ats.Domain.Airports;
using Ats.Domain.Flight;
using Ats.Domain.FlightInstance;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Domain.Booking
{
    public class AfricaThursadyDiscountServiceCriterion : IDiscountServiceCriterion
    {
        private readonly IRepository<FlightInstanceAggregate> _flightInstanceAggregateRepository;
        private readonly IRepository<FlightAggregate> _flightAggregateRepository;
        private readonly IRepository<AirportsAggregate> _airportsRepository;

        public AfricaThursadyDiscountServiceCriterion(
            IRepository<FlightInstanceAggregate> flightInstanceAggregateRepository,
            IRepository<FlightAggregate> flightAggregateRepository,
            IRepository<AirportsAggregate> airportsRepository)
        {
            _flightInstanceAggregateRepository = flightInstanceAggregateRepository ?? throw new ArgumentNullException(nameof(flightInstanceAggregateRepository));
            _flightAggregateRepository = flightAggregateRepository ?? throw new ArgumentNullException(nameof(flightAggregateRepository));
            _airportsRepository = airportsRepository ?? throw new ArgumentNullException(nameof(airportsRepository));
        }

        public string DiscountOfferName => "AfricaThursdayDiscount";

        public async Task<bool> CheckForAsync(BookingAggregate booking)
        {
            var flightInstance = await _flightInstanceAggregateRepository.GetAsync(booking.FlightInstanceId);
            var flight = await _flightAggregateRepository.GetAsync(flightInstance.FlightUid);
            var airports = await _airportsRepository.GetAsync(GlobalAirportsId.Id);
            return
                flightInstance.DepartureDate.DayOfWeek == DayOfWeek.Thursday &&
                airports.Airports.First(a => a.Code == flight.ArrivalAirport).Continent == Continent.Africa;
        }
    }
}
