using System;
using System.Threading.Tasks;

namespace Ats.Core.Domain
{
    public interface IRepository<TAggregate>
    {
        Task<TAggregate> GetAsync(Guid aggregateId);
        Task<TAggregate> GetAsync(string aggregateId);
        Task SaveAsync(Guid aggregateId, TAggregate aggregate, int expectedVersion);
        Task SaveAsync(string aggregateId, TAggregate aggregate, int expectedVersion);
    }
}
