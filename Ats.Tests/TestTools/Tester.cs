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
using System.Text.Json;
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
                await commandDispatcher.DispatchAsync(gwt.CommandToExecute);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            // THEN

            var ee = gwt.ExpectedException;
            if (ee == null)
            {
                if (thrownException != null)
                {
                    Assert.Fail(thrownException.ToString());
                }

                AssertEventStreamsAreEqual(gwt.ExpectedEvents, FakeRepositoryEventStore.SavedAggregateChanges);
            }
            else
            {
                if (thrownException == null)
                {
                    Assert.Fail($"Expected exception [{ee.ExceptionType.Name}] has not been thrown. Following events were produced instead: [{string.Join(", ", FakeRepositoryEventStore.SavedAggregateChanges.SelectMany(streamKvp => streamKvp.Value).Select(e => e.GetType().Name))}].");
                }

                if (ee.ExceptionType.IsAssignableFrom(thrownException.GetType()))
                {
                    if (ee.ExceptionMessage != null)
                    {
                        if (!thrownException.Message.Contains(ee.ExceptionMessage, StringComparison.OrdinalIgnoreCase))
                            Assert.Fail($"Expected [{ee.ExceptionType.Name}] has been thrown but message [{thrownException.Message}] doesn't match with [{ee.ExceptionMessage}].");
                    }
                }
                else
                {
                    Assert.Fail($"Unexpected exception has been thrown. Expected exception type was {ee.ExceptionType.Name} but thrown exception is of type {thrownException.GetType().Name}.");
                }
            }
        }

        private void AssertEventStreamsAreEqual(IDictionary<string, IEnumerable<IEvent>> expectedEventStreams, IDictionary<string, IEnumerable<IEvent>> publishedEventStreams)
        {
            if (expectedEventStreams == null || publishedEventStreams == null)
                Assert.Fail($"Expected and published events streams are not equal or both are nulls.");

            if (expectedEventStreams.Count != publishedEventStreams.Count)
                Assert.Fail($"Expected and published events streams are not equal. There is different number of expected and published event streams. Expected {expectedEventStreams.Count}, published {publishedEventStreams.Count}.");

            foreach (var expEvtStreamKvp in expectedEventStreams)
            {
                var streamName = expEvtStreamKvp.Key;
                var expectedEvents = expEvtStreamKvp.Value;
                if (!publishedEventStreams.TryGetValue(streamName, out IEnumerable<IEvent> publishedEvents))
                    Assert.Fail($"There is no published event stream with name {streamName} which is expected.");

                AssertEventsAreEqual(expectedEvents, publishedEvents);
            }
        }

        private void AssertEventsAreEqual(IEnumerable<IEvent> expectedEvents, IEnumerable<IEvent> publishedEvents)
        {
            AssertEventsAreEqual(expectedEvents.ToArray(), publishedEvents.ToArray());
        }

        private void AssertEventsAreEqual(IEvent[] expectedEvents, IEvent[] publishedEvents)
        {
            if (expectedEvents.Length != publishedEvents.Length)
                Assert.Fail($"Unexpected events published.{Environment.NewLine}Expected: {string.Join(",", expectedEvents.Select(ee => ee.GetType().Name))}{Environment.NewLine}But was: {string.Join(",", publishedEvents.Select(ee => ee.GetType().Name))}");

            for (var i = 0; i < expectedEvents.Length; ++i)
            {
                var expEvt = expectedEvents[i];
                var pubEvt = publishedEvents[i];
                var expEvtSer = Serialize(expEvt);
                var pubEvtSer = Serialize(pubEvt);
                Assert.AreEqual(expEvtSer, pubEvtSer, $"Published event of type {pubEvt.GetType().Name} has different data than expected one.");
            }
        }

        private string Serialize(object value)
        {
            return JsonSerializer.Serialize(value);
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
