using Ats.Application;
using Ats.Core.Domain;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Ats.Core.Commands;
using Ats.Domain;

namespace Ats.ComponentsRegistration
{
    public class ComponentsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Core

            container.Register(Classes.FromAssemblyContaining<IEvent>().Pick().WithServiceDefaultInterfaces().LifestyleTransient());
            container.Register(Types.FromAssemblyContaining<IEvent>().Where(t => t.Name.EndsWith("Factory")).Configure(c => c.AsFactory()).LifestyleTransient());

            // Application

            container.Register(Classes.FromAssemblyContaining<StartBookingCommand>().BasedOn(typeof(ICommandHandler<>)).WithServiceAllInterfaces().LifestyleTransient());
            container.Register(Classes.FromAssemblyContaining<StartBookingCommand>().Pick().WithServiceDefaultInterfaces().LifestyleTransient());

            // Domain

            container.Register(Classes.FromAssemblyContaining<BookingAggregate>().Where(t => t.Name.EndsWith("Service")).WithServiceSelf().LifestyleTransient());
            container.Register(Classes.FromAssemblyContaining<BookingAggregate>().Pick().WithServiceDefaultInterfaces().LifestyleTransient());

            // Infrastructure
        }
    }
}
