using Ats.Core.Commands;
using Ats.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ats.Tests.TestTools
{
    public class GivenWhenThen
    {
        public IDictionary<string, IEnumerable<IEvent>> InitializationEvents { get; private set; } = new Dictionary<string, IEnumerable<IEvent>>();
        public ICommand CommandToExecute { get; private set; }
        public IDictionary<string, IEnumerable<IEvent>> ExpectedEvents { get; private set; } = new Dictionary<string, IEnumerable<IEvent>>();
        public ExceptionExpectation ExpectedException { get; private set; }

        public GivenWhenThen Given(Guid eventStreamId, IEnumerable<IEvent> initializationEvents)
        {
            InitializationEvents.Add(eventStreamId.ToString(), initializationEvents);
            return this;
        }

        public GivenWhenThen Given(Guid eventStreamId, params IEvent[] initializationEvents)
        {
            InitializationEvents.Add(eventStreamId.ToString(), initializationEvents);
            return this;
        }

        public GivenWhenThen When(ICommand commandToExecute)
        {
            CommandToExecute = commandToExecute;
            return this;
        }

        public GivenWhenThen Then(Guid eventStreamId, IEnumerable<IEvent> expectedEvents)
        {
            Then(eventStreamId, expectedEvents.ToArray());
            return this;
        }

        public GivenWhenThen Then(Guid eventStreamId, params IEvent[] expectedEvents)
        {
            ExpectedEvents.Add(eventStreamId.ToString(), expectedEvents);
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
