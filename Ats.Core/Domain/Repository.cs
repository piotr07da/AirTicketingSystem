using System;
using System.Threading.Tasks;

namespace Ats.Core.Domain
{
    public class Repository<TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IChangeable
    {
        private readonly IAggregateFactory<TAggregate> _aggregateFactory;
        private readonly IRepositoryEventStore _repositoryEventStore;

        public Repository(
            IAggregateFactory<TAggregate> aggregateFactory,
            IRepositoryEventStore repositoryEventStore)
        {
            _aggregateFactory = aggregateFactory ?? throw new ArgumentNullException(nameof(aggregateFactory));
            _repositoryEventStore = repositoryEventStore ?? throw new ArgumentNullException(nameof(repositoryEventStore));
        }

        public async Task<TAggregate> GetAsync(Guid aggregateId)
        {
            return await GetAsync(aggregateId.ToString());
        }

        public async Task<TAggregate> GetAsync(string aggregateId)
        {
            var streamName = FormatStreamName(aggregateId);
            var events = await _repositoryEventStore.ReadAsync(streamName);
            return _aggregateFactory.Create(events);
        }

        public async Task SaveAsync(Guid aggregateId, TAggregate aggregate, int expectedVersion)
        {
            await SaveAsync(aggregateId.ToString(), aggregate, expectedVersion);
        }

        public async Task SaveAsync(string aggregateId, TAggregate aggregate, int expectedVersion)
        {
            var streamName = FormatStreamName(aggregateId);
            var newEvents = aggregate.Changes.Get();
            await _repositoryEventStore.WriteAsync(streamName, newEvents, expectedVersion);
            aggregate.Changes.Clear();
        }

        private string FormatStreamName(string aggregateId)
        {
            return $"{typeof(TAggregate).Name}___{aggregateId}";
        }
    }
}
