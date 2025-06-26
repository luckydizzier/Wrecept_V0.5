# Plugin API

Wrecept supports optional menu extensions through simple plug-in assemblies. Plugins are loaded from a `Plugins` folder located next to the application executable.

## Implementing `IMenuPlugin`

Create a .NET class library that references `Wrecept.Core`. Each plugin must define a public class implementing the interface:

```csharp
public interface IMenuPlugin
{
    string MenuHeader { get; }
    void Execute();
}
```

`MenuHeader` provides the menu label shown under **Súgó → Bővítmények**. `Execute` is called when the user selects the item.

The plugin class must have a parameterless constructor. It may show dialogs or perform any action allowed by standard WPF APIs.

## Building and Installing

1. Name the assembly `Wrecept.Plugin.<YourPlugin>`.
2. Build the project in Release mode.
3. Copy the resulting `.dll` to the `Plugins` directory beside `Wrecept.exe`.

On the next start the loader discovers the new plugin and adds its menu item automatically.

A minimal example is provided in `src/Wrecept.Plugin.Greeting`.

## Upcoming Extensions
Later milestones may introduce new interfaces for list or toolbar plugins.
These will follow the same discovery rules and live under the `Wrecept.Core.Plugins` namespace.
