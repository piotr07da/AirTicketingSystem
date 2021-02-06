using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ats.Core.Domain
{
    public interface IRepositoryEventStore
    {
        Task<IEvent[]> ReadAsync(string streamName);
        Task WriteAsync(string streamName, IEnumerable<IEvent> events, int expectedVersion);
    }
}