using Ats.Core.Domain;
using Ats.Domain.Customer;
using Ats.Domain.FlightInstance;
using System;
using System.Threading.Tasks;

namespace Ats.Domain.Booking
{
    public class BirthdayDiscountServiceCriterion : IDiscountServiceCriterion
    {
        private readonly IRepository<CustomerAggregate> _customerRepository;
        private readonly IRepository<FlightInstanceAggregate> _flightInstanceAggregateRepository;

        public BirthdayDiscountServiceCriterion(
            IRepository<CustomerAggregate> customerRepository,
            IRepository<FlightInstanceAggregate> flightInstanceAggregateRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _flightInstanceAggregateRepository = flightInstanceAggregateRepository ?? throw new ArgumentNullException(nameof(flightInstanceAggregateRepository));
        }

        public string DiscountOfferName => "BirthdayDiscount";

        public async Task<bool> CheckForAsync(BookingAggregate booking)
        {
            var customer = await _customerRepository.GetAsync(booking.CustomerId);
            var flightInstance = await _flightInstanceAggregateRepository.GetAsync(booking.FlightInstanceId);
            return
                customer.Birthday.Month == flightInstance.DepartureDate.Month &&
                customer.Birthday.Day == flightInstance.DepartureDate.Day;
        }
    }
}
