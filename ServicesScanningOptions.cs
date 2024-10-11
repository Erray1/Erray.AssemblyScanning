

namespace Erray.ServicesScanning
{
    public class ServicesScanningOptions
    {
        public bool IncludeNestedNamespaces { get; set; }
        public static ServicesScanningOptions Default => new()
        {
            IncludeNestedNamespaces = true,
        };
    }
}
