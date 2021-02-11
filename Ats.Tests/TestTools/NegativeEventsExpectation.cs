using Ats.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Ats.Tests.TestTools
{
    public class NegativeEventsExpectation : IEventsExpectation
    {
        private readonly Func<IEvent, bool>[] _unexpectedEventQualifiers;

        public NegativeEventsExpectation(Func<IEvent, bool>[] unexpectedEventQualifiers)
        {
            _unexpectedEventQualifiers = unexpectedEventQualifiers ?? throw new ArgumentNullException(nameof(unexpectedEventQualifiers));
        }

        public void Verify(IEnumerable<IEvent> publishedEvents)
        {
            foreach (var e in publishedEvents)
            {
                for (int ueqIx = 0; ueqIx < _unexpectedEventQualifiers.Length; ++ueqIx)
                {
                    if (_unexpectedEventQualifiers[ueqIx](e))
                    {
                        Assert.Fail($"Event specified below matches unexpected event qualifier with index {ueqIx}.{Environment.NewLine}{EventSerializer.Serialize(e)}");
                    }
                }
            }
        }
    }

    public delegate bool ExcludedEventQualifier<in TEvent>(TEvent e) where TEvent: IEvent;
}
