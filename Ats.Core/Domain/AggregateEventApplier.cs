using System;
using System.Collections.Generic;

namespace Ats.Core.Domain
{
    public class AggregateEventApplier : IAggregateEventApplier
    {
        private readonly IEventApplierActionsExtractor _eventApplierActionsExtractor;

        private IChangable _registeredAggregate;
        private IDictionary<Type, EventApplierAction> _eventApplierActions;

        public AggregateEventApplier(IEventApplierActionsExtractor eventApplierActionsExtractor)
        {
            _eventApplierActionsExtractor = eventApplierActionsExtractor ?? throw new ArgumentNullException(nameof(eventApplierActionsExtractor));
        }

        public void Register(IChangable aggregate)
        {
            if (_registeredAggregate != null)
                throw new InvalidOperationException($"Cannot register an aggregate. Another aggregate has been registered first. Probable cause - {nameof(AggregateEventApplier)} is singleton, you need to create new instance per each aggregate.");
            _eventApplierActions = _eventApplierActionsExtractor.Extract(aggregate);
            _registeredAggregate = aggregate;
        }

        public void ApplyNewEvents(IEnumerable<IEvent> events)
        {
            foreach (var evt in events)
            {
                ApplyNewEvent(evt);
            }
        }

        public void ApplyNewEvent(IEvent @event)
        {
            ApplyEvent(@event, true);
        }

        public void ApplyExistingEvents(IEnumerable<IEvent> events)
        {
            foreach (var evt in events)
            {
                ApplyExistingEvent(evt);
            }
        }

        public void ApplyExistingEvent(IEvent @event)
        {
            ApplyEvent(@event, false);
        }

        private void ApplyEvent(IEvent @event, bool isNew)
        {
            var eventType = @event.GetType();

            if (!_eventApplierActions.TryGetValue(eventType, out EventApplierAction eventApplier))
            {
                if (isNew)
                    throw new InvalidOperationException($"Aggregate [{_registeredAggregate.GetType().Name}] produced new event of type [{eventType.Name}] but this aggregate has no [void Apply({eventType.Name} e)] method. Aggregate doesn't have to handle every event but it has to handle at least events produced by itself.");
                else
                    return;
            }

            eventApplier(@event);

            if (isNew)
            {
                _registeredAggregate.Changes.Add(@event);
            }
        }
    }
}
