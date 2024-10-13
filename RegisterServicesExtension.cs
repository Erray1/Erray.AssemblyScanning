using Erray.ServicesScanning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Erray.AssemblyScanning
{
    /// <summary>
    /// Register services automaticaly. To start, create some class implementing IServicesRegistrationMark
    /// interface and place it in assembly, from which services must be registred
    /// </summary>
    public static class RegisterServicesExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAssemblyMark">Class whose namespace includes services to be registred</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="opt">Registration options</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection ScanAndRegisterServices<TAssemblyMark>(this IServiceCollection services, ServicesScanningOptions? opt = null)
            where TAssemblyMark : IServicesRegistrationMark
        {
            if (opt is null) opt = ServicesScanningOptions.Default;
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyMarkType = typeof(TAssemblyMark);
            var namespaceName = assemblyMarkType.Namespace!;
            var types = assembly.GetTypes()
                .Where(x => x.IsClass
                && x.GetCustomAttribute(typeof(SuppressAutomaticRegistration)) is null
                && !x.IsAbstract
                && !(x.IsAbstract && x.IsSealed)
                && !x.IsNestedPrivate
                && !string.IsNullOrEmpty(x.Namespace)
                && !x.IsAssignableTo(typeof(IServicesRegistrationMark))
                && !x.IsAssignableTo(typeof(BackgroundService)));
            types = opt.IncludeNestedNamespaces ?
                types.Where(x => x.Namespace!.Contains(namespaceName))
                : types.Where(x => string.Equals(x.Namespace, namespaceName));

            var filteredTypes = types
                .Select(x => new
                {
                    Implementation = x,
                    Interfaces = x.GetInterfaces()
                    .Where(x => x.GetCustomAttribute(typeof(SuppressAutomaticRegistration)) is null)
                });

            foreach (var serviceType in filteredTypes)
            {
                if (!serviceType.Interfaces.Any())
                {
                    services.AddScoped(serviceType.Implementation);
                    continue;
                }
                foreach (var interfaceType in serviceType.Interfaces)
                {
                    services.AddScoped(interfaceType, serviceType.Implementation);
                }
            }
            return services;
        }
    }
}
