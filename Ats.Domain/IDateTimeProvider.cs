using System;

namespace Ats.Domain
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
