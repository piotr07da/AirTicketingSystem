using System;
using System.Collections.Generic;

namespace Ats.Core.Domain
{
    public class AggregateFactory<TAggregate> : IAggregateFactory<TAggregate>
        where TAggregate : class, IChangable
    {
        private readonly IAggregateEventApplierFactory _aggregateEventApplierFactory;
        private readonly IAggregateFactory _aggregateFactory;

        public AggregateFactory(IAggregateEventApplierFactory aggregateEventApplierFactory, IAggregateFactory aggregateFactory)
        {
            _aggregateEventApplierFactory = aggregateEventApplierFactory ?? throw new ArgumentNullException(nameof(aggregateEventApplierFactory));
            _aggregateFactory = aggregateFactory ?? throw new ArgumentNullException(nameof(aggregateFactory));
        }

        public TAggregate Create()
        {
            return Create(null);
        }

        public TAggregate Create(IEnumerable<IEvent> events)
        {
            var eventApplier = _aggregateEventApplierFactory.Create();
            var aggregate = _aggregateFactory.Create<TAggregate>(eventApplier);
            eventApplier.Register(aggregate);

            if (events != null)
                eventApplier.ApplyExistingEvents(events);

            return aggregate;
        }
    }
}
