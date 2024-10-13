

namespace Erray.ServicesScanning
{
    /// <summary>
    /// Options for automatic services registry
    /// </summary>
    public class ServicesScanningOptions
    {
        /// <summary>
        /// Include nested namespaces. If IServicesRegistrationMark is places in MainNamespace and property is true,
        /// then services from MainNamespace.SubNamespace will be registred too
        /// </summary>
        public bool IncludeNestedNamespaces { get; set; }
        public static ServicesScanningOptions Default => new()
        {
            IncludeNestedNamespaces = true,
        };
    }
}
