﻿using System.Collections.Generic;

namespace Ats.Core.Domain
{
    public interface IAggregateEventApplier
    {
        void Register(IChangeable aggregate);
        void ApplyNewEvent(IEvent @event);
        void ApplyExistingEvents(IEnumerable<IEvent> events);
    }
}
