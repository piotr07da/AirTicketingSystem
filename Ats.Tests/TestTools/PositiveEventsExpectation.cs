using Ats.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ats.Tests.TestTools
{
    public class PositiveEventsExpectation : IEventsExpectation
    {
        private readonly IEnumerable<IEvent> _expectedEvents;
        private readonly PositiveEventsExpectationType _positiveEventsExpectationType;

        public PositiveEventsExpectation(IEnumerable<IEvent> expectedEvents, PositiveEventsExpectationType positiveEventsExpectationType)
        {
            _expectedEvents = expectedEvents ?? throw new ArgumentNullException(nameof(expectedEvents));
            _positiveEventsExpectationType = positiveEventsExpectationType;
        }

        public void Verify(IEnumerable<IEvent> publishedEvents)
        {
            AssertEventsAreEqual(_expectedEvents, publishedEvents);
        }

        private void AssertEventsAreEqual(IEnumerable<IEvent> expectedEvents, IEnumerable<IEvent> publishedEvents)
        {
            AssertEventsAreEqual(expectedEvents.ToArray(), publishedEvents.ToArray());
        }

        private void AssertEventsAreEqual(IEvent[] expectedEvents, IEvent[] publishedEvents)
        {
            if (_positiveEventsExpectationType == PositiveEventsExpectationType.Identical && expectedEvents.Length != publishedEvents.Length)
                Assert.Fail($"Expected and published events are not the same.{Environment.NewLine}Expected: [{string.Join(",", expectedEvents.Select(ee => ee.GetType().Name))}]{Environment.NewLine}But was: [{string.Join(",", publishedEvents.Select(ee => ee.GetType().Name))}]");
            else if (_positiveEventsExpectationType == PositiveEventsExpectationType.Contains && expectedEvents.Length > publishedEvents.Length)
                Assert.Fail($"There is more expected events than published events.{Environment.NewLine}Expected: [{string.Join(",", expectedEvents.Select(ee => ee.GetType().Name))}]{Environment.NewLine}But was: [{string.Join(",", publishedEvents.Select(ee => ee.GetType().Name))}]");

            var pei = -1;
            IEvent pubEvt = null;
            string pubEvtSer = null;
            bool matchFound;

            for (var eei = 0; eei < expectedEvents.Length; ++eei)
            {
                var expEvt = expectedEvents[eei];
                var expEvtSer = EventSerializer.Serialize(expEvt);

                do
                {
                    ++pei;

                    if (pei > publishedEvents.Length)
                    {
                        Assert.Fail($"Expected event not found. Expected event was:{Environment.NewLine}{expEvtSer}");
                    }

                    pubEvt = publishedEvents[pei];
                    pubEvtSer = EventSerializer.Serialize(pubEvt);

                    matchFound = expEvtSer.Equals(pubEvtSer);

                    if (_positiveEventsExpectationType == PositiveEventsExpectationType.Identical && !matchFound)
                    {
                        Assert.AreEqual(expEvtSer, pubEvtSer, $"Published event of type {pubEvt.GetType().Name} has different data than expected one of type {expEvt.GetType().Name}.");
                    }
                }
                while (!matchFound);
            }
        }
    }
}
