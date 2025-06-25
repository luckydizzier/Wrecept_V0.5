# Plugin Architecture Specification

## Context
Wrecept aims for extensibility via optional modules. Plug-ins will allow feature expansion without modifying the core application.

## Loading Strategy
- Plugins are discovered from a `Plugins` folder next to the executable.
- Assemblies matching `Wrecept.Plugin.*.dll` are loaded at startup.
- Each assembly must contain a class implementing `IMenuPlugin`.

## Plugin Interface
```csharp
public interface IMenuPlugin
{
    string MenuHeader { get; }
    void Execute();
}
```
- The plugin provides a menu label and action invoked when selected.

## Integration Notes
- Plugins can reference the `Wrecept.Core` project for shared types.
- The loader creates each plugin using a parameterless constructor.
- Plugins may display dialogs using standard WPF APIs.

