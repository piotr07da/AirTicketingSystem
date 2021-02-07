using Ats.Core.Domain;
using Ats.Domain.Flight;
using Ats.Domain.FlightInstance;
using System;
using System.Threading.Tasks;

namespace Ats.Domain.Booking
{
    public class AfricaThursadyDiscountServiceCriterion : IDiscountServiceCriterion
    {
        private readonly IRepository<FlightInstanceAggregate> _flightInstanceAggregateRepository;
        private readonly IRepository<FlightAggregate> _flightAggregateRepository;

        public AfricaThursadyDiscountServiceCriterion(
            IRepository<FlightInstanceAggregate> flightInstanceAggregateRepository,
            IRepository<FlightAggregate> flightAggregateRepository)
        {
            _flightInstanceAggregateRepository = flightInstanceAggregateRepository ?? throw new ArgumentNullException(nameof(flightInstanceAggregateRepository));
            _flightAggregateRepository = flightAggregateRepository ?? throw new ArgumentNullException(nameof(flightAggregateRepository));
        }

        public string DiscountOfferName => "AfricaThursdayDiscount";

        public async Task<bool> CheckForAsync(BookingAggregate booking)
        {
            var flightInstance = await _flightInstanceAggregateRepository.GetAsync(booking.FlightInstanceId);
            var flight = await _flightAggregateRepository.GetAsync(flightInstance.FlightUid);
            return
                flightInstance.DepartureDate.DayOfWeek == DayOfWeek.Thursday;
        }
    }
}
