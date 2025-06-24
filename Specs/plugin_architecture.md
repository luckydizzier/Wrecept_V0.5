# Plugin Architecture Specification

## Context
Wrecept aims for extensibility via optional modules. Plug-ins will allow feature expansion without modifying the core application.

## Loading Strategy
- Plugins are discovered from a `Plugins` folder next to the executable.
- Assemblies matching `Wrecept.Plugin.*.dll` are loaded at startup.
- Each assembly must contain a class implementing `IPlugin`.

## Plugin Interface
```csharp
public interface IPlugin
{
    void ConfigureServices(IServiceCollection services);
}
```
- The method is called after core service registration.
- Dependencies are provided via constructor injection.

## Dependency Injection Guidelines
- The core app uses Microsoft.Extensions.DependencyInjection.
- Plugins should register their own services with scoped or singleton lifetime as needed.
- Avoid ServiceLocator patterns.

