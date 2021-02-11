using Ats.Core.Domain;
using System.Collections.Generic;

namespace Ats.Tests.TestTools
{
    public interface IEventsExpectation
    {
        void Verify(IEnumerable<IEvent> publishedEvents);
    }
}
