using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ats.Core.Domain
{
    public class EventApplierActionsExtractor : IEventApplierActionsExtractor
    {
        public IDictionary<Type, EventApplierAction> Extract(object extractionSource)
        {
            var extractionSourceType = extractionSource.GetType();

            var applyMethods = extractionSourceType
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == "Apply");

            var extractedApplyDelegates = new Dictionary<Type, EventApplierAction>();

            var baseEventType = typeof(IEvent);

            foreach (var m in applyMethods)
            {
                if (m.ReturnType != typeof(void))
                    throw new Exception("All aggregate Apply methods has to have void return type.");

                var prms = m.GetParameters();

                if (prms.Length != 1)
                    throw new Exception("All aggregate Apply methods has to have exactly one argument.");

                var prm = prms[0];

                if (!baseEventType.IsAssignableFrom(prm.ParameterType))
                    throw new Exception($"All aggregate Apply methods has to have exactly one argument of type that implements {baseEventType.FullName}.");

                extractedApplyDelegates.Add(prm.ParameterType, evt => m.Invoke(extractionSource, new[] { evt }));
            }

            return extractedApplyDelegates;
        }
    }
}
