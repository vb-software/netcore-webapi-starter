using AutoMapper;
using RootNamespace.Entities.AutoMapper.Profiles;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RootNamespace.Entities.Interfaces;
#if (useMongoDB)
using RootNamespace.Entities.Settings;
#endif
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
            // Setup DTO validators
            SetupDTOValidators(services);
        }

        private static void SetupMappings(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMapperMarker));
        }

        private static void SetupConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            #if (useMongoDB)
            var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            services.AddSingleton(mongoDbSettings);
            #endif
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

        private static void SetupDTOValidators(IServiceCollection services)
        {
            // Due to the open generic being used to identify concrete implementations using IValidator<T>...
            // Restricting scan to DTO namespace to that additional implementations are not wired up
            // as this was calling an aggregation exception
            services.Scan(scan =>
                scan
                    .FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)).InNamespaces("Entities.DTO"))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
            );
        }
    }
}