using Ats.Core.Commands;
using Ats.Core.Domain;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Releasers;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Tests.TestTools
{
    public class Tester
    {
        public static async Task TestAsync(Func<GivenWhenThen, GivenWhenThen> gwtPreparationAction)
        {
            var tester = new Tester();
            var gwt = gwtPreparationAction(new GivenWhenThen());
            await tester.InternalTestAsync(gwt);
        }

        public static async Task TestAsync(GivenWhenThen gwt)
        {
            var tester = new Tester();
            await tester.InternalTestAsync(gwt);
        }

        private async Task InternalTestAsync(GivenWhenThen gwt)
        {
            var container = BoostrapContainer();

            // GIVEN

            FakeRepositoryEventStore.Reset(gwt.InitializationEvents);

            // WHEN

            var commandDispatcher = container.Resolve<ICommandDispatcher>();

            Exception thrownException = null;
            try
            {
                foreach (var cte in gwt.CommandsToExecute)
                {
                    await commandDispatcher.DispatchAsync(cte);
                }
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            // THEN

            var expectedException = gwt.ExpectedException;
            if (expectedException == null)
            {
                if (thrownException != null)
                {
                    Assert.Fail(thrownException.ToString());
                }

                AssertEventStreamsMeetExpectations(gwt.ExpectedEvents, FakeRepositoryEventStore.SavedAggregateChanges);
            }
            else
            {
                if (thrownException == null)
                {
                    Assert.Fail($"Expected exception [{expectedException.ExceptionType.Name}] has not been thrown. Following events were produced instead: [{string.Join(", ", FakeRepositoryEventStore.SavedAggregateChanges.SelectMany(streamKvp => streamKvp.Value).Select(e => e.GetType().Name))}].");
                }

                if (expectedException.ExceptionType.IsAssignableFrom(thrownException.GetType()))
                {
                    if (expectedException.ExceptionMessage != null)
                    {
                        if (!thrownException.Message.Contains(expectedException.ExceptionMessage, StringComparison.OrdinalIgnoreCase))
                            Assert.Fail($"Expected [{expectedException.ExceptionType.Name}] has been thrown but message [{thrownException.Message}] doesn't match with [{expectedException.ExceptionMessage}].");
                    }
                }
                else
                {
                    Assert.Fail($"Unexpected exception has been thrown. Expected exception type was {expectedException.ExceptionType.Name} but thrown exception is of type {thrownException.GetType().Name} and exception is {thrownException.ToString()}.");
                }
            }
        }

        private void AssertEventStreamsMeetExpectations(IDictionary<string, IEventsExpectation> eventsExpectations, IDictionary<string, IEnumerable<IEvent>> publishedEventStreams)
        {
            if (eventsExpectations is null) throw new ArgumentNullException(nameof(eventsExpectations));
            if (publishedEventStreams is null) throw new ArgumentNullException(nameof(publishedEventStreams));

            if (eventsExpectations.Count != publishedEventStreams.Count)
                Assert.Fail($"Streams with events expectations and streams with published events are not same streams. There is different number of streams with expectations and streams with published events. Expected {eventsExpectations.Count}, published {publishedEventStreams.Count}.");

            foreach (var expEvtStreamKvp in eventsExpectations)
            {
                var streamName = expEvtStreamKvp.Key;
                var eventsExpectation = expEvtStreamKvp.Value;

                if (!publishedEventStreams.TryGetValue(streamName, out IEnumerable<IEvent> publishedEvents))
                    Assert.Fail($"There is no published event stream with name {streamName} which has events expectation.");

                eventsExpectation.Verify(publishedEvents);
            }
        }

        private IWindsorContainer BoostrapContainer()
        {
            var container = new WindsorContainer();

#pragma warning disable CS0618 // No. It is not obsolete. This whole functionality of release policies in Castle.Windsor is pure nonsense and completely breaks SRP. Therefore there is the only one valid option here - NoTrackingReleasePolicy to suppress that BS.
            container.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
#pragma warning restore CS0618
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            container.Install(Castle.Windsor.Installer.Configuration.FromXmlFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "windsor.xml")));

            return container;
        }
    }
}
