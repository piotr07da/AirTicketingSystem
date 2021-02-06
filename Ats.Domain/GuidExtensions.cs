using System;

namespace Ats.Domain
{
    public static class GuidExtensions
    {
        public static bool IsUndefined(this Guid guid) => guid == Guid.Empty;
    }
}
