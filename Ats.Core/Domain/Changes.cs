using System.Collections.Generic;
using System.Linq;

namespace Ats.Core.Domain
{
    public class Changes
    {
        private readonly IList<IEvent> _changes = new List<IEvent>();

        public int Count => _changes.Count;

        public IEvent[] Get() => _changes.ToArray();

        public void Add(IEvent @event) => _changes.Add(@event);

        public void Clear() => _changes.Clear();
    }
}