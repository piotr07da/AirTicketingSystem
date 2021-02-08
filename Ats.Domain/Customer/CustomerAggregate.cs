using Ats.Core.Domain;
using Ats.Domain.FlightInstance;
using System;

namespace Ats.Domain.Customer
{
    public class CustomerAggregate : IChangeable
    {
        private readonly IAggregateEventApplier _aggregateEventApplier;

        private CustomerId _id;
        private DateTime _birthday;

        public CustomerAggregate(IAggregateEventApplier aggregateEventApplier)
        {
            _aggregateEventApplier = aggregateEventApplier ?? throw new ArgumentNullException(nameof(aggregateEventApplier));
        }

        public Changes Changes { get; } = new Changes();

        public CustomerId Id => _id;
        public DateTime Birthday => _birthday;

        private void Apply(CustomerRegisteredEvent e)
        {
            _id = e.CustomerId;
            _birthday = e.Birthday;
        }

    }
}
