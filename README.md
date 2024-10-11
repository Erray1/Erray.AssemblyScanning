# Erray.ServicesScanning

### To use this library, create a new class in namespace, in which you want to register your services. The class must implement IServicesRegistrationMark

### Then call ScanAndRegisterServices() method on IServiceCollection.

Options:

<code>bool IncludeNestedNamespaces</code>: if true, services from all sub-namespaces of chosen namespace will be registred.
Example: Registration mark is placed in <code>Example</code> namespace. Then services from Example.SubNamespace will be registred too.
