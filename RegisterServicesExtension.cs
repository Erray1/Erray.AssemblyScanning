using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Erray.AssemblyScanning
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection ScanAndRegisterServices<TAssemblyMark>(this IServiceCollection services)
            where TAssemblyMark : IServicesRegistrationMark
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyMarkType = typeof(TAssemblyMark);
            var namespaceName = assemblyMarkType.Namespace!;
            var types = assembly.GetTypes()
                .Where(x => x.IsClass 
                && !x.IsAbstract 
                && !(x.IsAbstract && x.IsSealed) // static
                && !x.IsNestedPrivate
                && !string.IsNullOrEmpty(x.Namespace) 
                && x.Namespace!.Contains(namespaceName)
                && !x.IsAssignableTo(typeof(IServicesRegistrationMark))
                && !x.IsAssignableTo(typeof(BackgroundService)))
                .Select(x => new
                {
                    Implementation = x,
                    Interfaces = x.GetInterfaces()
                })
                .ToList();

            foreach (var serviceType in  types)
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
