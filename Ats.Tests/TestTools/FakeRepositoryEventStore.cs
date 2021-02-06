using Ats.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ats.Tests.TestTools
{
    public class FakeRepositoryEventStore : IRepositoryEventStore
    {
        [ThreadStatic]
        private static IDictionary<string, IEnumerable<IEvent>> _historyEvents;
        [ThreadStatic]
        private static IDictionary<string, IEnumerable<IEvent>> _savedAggregateChanges;

        public static IDictionary<string, IEnumerable<IEvent>> SavedAggregateChanges => _savedAggregateChanges;

        public static void Reset(IDictionary<string, IEnumerable<IEvent>> initializationEvents)
        {
            _historyEvents = new Dictionary<string, IEnumerable<IEvent>>();
            _savedAggregateChanges = new Dictionary<string, IEnumerable<IEvent>>();
            foreach (var initEvtsKvp in initializationEvents)
                _historyEvents.Add(initEvtsKvp.Key, initEvtsKvp.Value);
        }

        public Task<IEvent[]> ReadAsync(string streamName)
        {
            var guid = ExtractGuidFromStreamName(streamName);
            if (_historyEvents.TryGetValue(guid, out IEnumerable<IEvent> evts))
                return Task.FromResult(evts.ToArray());

            return Task.FromResult(null as IEvent[]);
        }

        public Task WriteAsync(string streamName, IEnumerable<IEvent> events, int expectedVersion)
        {
            var guid = ExtractGuidFromStreamName(streamName);

            int currentVersion;
            if (_historyEvents.TryGetValue(guid, out IEnumerable<IEvent> evts))
                currentVersion = evts.Count();
            else
                currentVersion = 0;

            if (expectedVersion != currentVersion)
                throw new Exception($"Cannot write events because expected version is {expectedVersion} and current version is {currentVersion}.");

            _savedAggregateChanges[guid] = events.ToList(); // makes copy o events collection
            return Task.CompletedTask;
        }

        private string ExtractGuidFromStreamName(string streamName)
        {
            return Regex.Replace(streamName, "^[A-Za-z0-9]+___", string.Empty);
        }
    }
}
