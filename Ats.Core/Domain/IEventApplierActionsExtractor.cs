using System;
using System.Collections.Generic;

namespace Ats.Core.Domain
{
    public interface IEventApplierActionsExtractor
    {
        IDictionary<Type, EventApplierAction> Extract(object extractionSource);
    }
}
