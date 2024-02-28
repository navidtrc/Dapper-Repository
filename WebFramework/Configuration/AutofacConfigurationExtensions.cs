using Autofac;
using Common;
using DAL.Dapper;
using Entities;
using Services;

namespace WebFramework.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public static void AddServices(this ContainerBuilder containerBuilder)
        {
            //RegisterType > As > Liftetime
            containerBuilder.RegisterGeneric(typeof(DAL.Dapper.Persistence.Repositories.Repository<>)).As(typeof(DAL.Dapper.Infrastructure.Repositories.IRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(DAL.Dapper.Persistence.Repositories.Query<>)).As(typeof(DAL.Dapper.Infrastructure.Repositories.IQuery<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            //var dataAssembly = typeof(ApplicationDbContext).Assembly;
            var dataAssembly = typeof(DapperContext).Assembly;
            var servicesAssembly = typeof(AccessToken).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        //We don't need this since Autofac updates for ASP.NET Core 3.0+ Generic Hosting
        //public static IServiceProvider BuildAutofacServiceProvider(this IServiceCollection services)
        //{
        //    var containerBuilder = new ContainerBuilder();
        //    containerBuilder.Populate(services);
        //
        //    //Register Services to Autofac ContainerBuilder
        //    containerBuilder.AddServices();
        //
        //    var container = containerBuilder.Build();
        //    return new AutofacServiceProvider(container);
        //}
    }
}
