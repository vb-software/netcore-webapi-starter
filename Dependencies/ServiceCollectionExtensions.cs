using AutoMapper;
using Entities.AutoMapper.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RootNamespace.Entities.Interfaces;
using RootNamespace.Repositories.Interfaces;
using RootNamespace.Services.Interfaces;

namespace RootNamespace.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Initialize all auto mappings
            SetupMappings(services);
            // Setup singletons to represent various configurations
            SetupConfigurations(services, configuration);
            // Setup stateless singletons that implement ISingleton
            SetupSingletons(services);
            // Setup concrete repository classes that implement IRepository
            SetupRepositories(services);
            // Setup concrete service classes that implement IService
            SetupServices(services);
        }

        private static void SetupMappings(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMapperMarker));
        }

        private static void SetupConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            // Here you can setup singletons to represent configurations
            // Example
            // var settings = configuration.GetSection("MySettings").Get<MySettings>();
            //
            // services.AddSingleton(settings);
        }

        private static void SetupServices(IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo<IService>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
            );
        }

        private static void SetupRepositories(IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo<IRepository>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
            );
        }

        private static void SetupSingletons(IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo<ISingleton>())
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime()
            );
        }
    }
}