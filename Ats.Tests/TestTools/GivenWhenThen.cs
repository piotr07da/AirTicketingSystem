using Ats.Core.Commands;
using Ats.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ats.Tests.TestTools
{
    // TODO - trzeba by dodać następujące metody (i ich obsługę w klasie Tester):
    // - ThenNot - dla walidacji czy dane eventy nie wystąpiły (być może także wersja z dokładnością tylko do typu eventu)
    // - ThenContains - dla walidacji czy dany event wystapił
    // a aktualnie zaimplementowana metoda Then powinna zmienić nazwę na ThenExact - dla walidacji czy wystąpiły dokładnie te eventy, które są wskazane. Mogłba by się też pojawić metoda ThenExactAnyOrder - w tym samym celu co ThenExact ale bez walidacji kolejności eventów.

    public class GivenWhenThen
    {
        public IDictionary<string, IEnumerable<IEvent>> InitializationEvents { get; private set; } = new Dictionary<string, IEnumerable<IEvent>>();
        public IList<ICommand> CommandsToExecute { get; private set; } = new List<ICommand>();
        public IDictionary<string, IEventsExpectation> ExpectedEvents { get; private set; } = new Dictionary<string, IEventsExpectation>();
        public ExceptionExpectation ExpectedException { get; private set; }

        public GivenWhenThen Given(Guid eventStreamId, IEnumerable<IEvent> initializationEvents)
        {
            Given(eventStreamId, initializationEvents.ToArray());
            return this;
        }

        public GivenWhenThen Given(Guid eventStreamId, params IEvent[] initializationEvents)
        {
            var key = eventStreamId.ToString();

            if (InitializationEvents.TryGetValue(key, out IEnumerable<IEvent> currentInitializationEvents))
            {
                InitializationEvents[key] = currentInitializationEvents.Union(initializationEvents).ToArray();
            }
            else
            {
                InitializationEvents.Add(key, initializationEvents);
            }
            return this;
        }

        public GivenWhenThen When(ICommand commandToExecute)
        {
            CommandsToExecute.Add(commandToExecute);
            return this;
        }

        public GivenWhenThen ThenIdentical(Guid eventStreamId, IEnumerable<IEvent> expectedEvents)
        {
            ThenIdentical(eventStreamId, expectedEvents.ToArray());
            return this;
        }

        public GivenWhenThen ThenIdentical(Guid eventStreamId, params IEvent[] expectedEvents)
        {
            ExpectedEvents.Add(eventStreamId.ToString(), new PositiveEventsExpectation(expectedEvents, PositiveEventsExpectationType.Identical));
            return this;
        }

        public GivenWhenThen ThenContains(Guid eventStreamId, IEnumerable<IEvent> expectedEvents)
        {
            ThenContains(eventStreamId, expectedEvents.ToArray());
            return this;
        }

        public GivenWhenThen ThenContains(Guid eventStreamId, params IEvent[] expectedEvents)
        {
            ExpectedEvents.Add(eventStreamId.ToString(), new PositiveEventsExpectation(expectedEvents, PositiveEventsExpectationType.Contains));
            return this;
        }

        public GivenWhenThen ThenDoesntContain(Guid eventStreamId, params Func<IEvent, bool>[] unexpectedEventQualifiers)
        {
            ExpectedEvents.Add(eventStreamId.ToString(), new NegativeEventsExpectation(unexpectedEventQualifiers));
            return this;
        }

        public GivenWhenThen Throws<TException>(string messagePattern = null)
            where TException : Exception
        {
            if (ExpectedException != null)
                throw new InvalidOperationException("There is expected exception associated with this GWT. You cannot expect another exception. Remove previous expectation if needed.");

            ExpectedException = new ExceptionExpectation(typeof(TException), messagePattern);

            return this;
        }
    }
}
