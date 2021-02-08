using Ats.Core.Domain;
using Ats.Domain;
using Ats.Tests.TestTools;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Ats.Tests.Di
{
    public class ComponentsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRepositoryEventStore>().ImplementedBy<FakeRepositoryEventStore>().IsDefault().LifestyleTransient());
            container.Register(Component.For<IDateTimeProvider>().ImplementedBy<FakeDateTimeProvider>().IsDefault().LifestyleTransient());
            container.Register(Component.For<ITenant>().ImplementedBy<FakeTenant>().IsDefault().LifestyleTransient());
        }
    }
}
