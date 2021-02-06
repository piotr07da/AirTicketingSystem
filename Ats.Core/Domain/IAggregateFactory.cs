using System.Collections.Generic;

namespace Ats.Core.Domain
{
    public interface IAggregateFactory<TAggregate>
        where TAggregate : class
    {
        TAggregate Create();

        TAggregate Create(IEnumerable<IEvent> events);
    }

    public interface IAggregateFactory
    {
        TAggregate Create<TAggregate>(IAggregateEventApplier aggregateEventApplier);
    }
}
