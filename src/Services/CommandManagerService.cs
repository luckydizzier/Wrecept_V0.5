using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Services;

public class CommandManagerService
{
    private UIElement? _activeView;
    private readonly List<InputBinding> _bindings = new();

    public void SetActiveView(UIElement view)
    {
        if (_activeView is not null)
        {
            foreach (var b in _bindings)
                _activeView.InputBindings.Remove(b);
            _bindings.Clear();
        }

        _activeView = view;

        if (view.DataContext is not null)
        {
            foreach (var cmd in ExtractCommands(view.DataContext))
            {
                var binding = new KeyBinding(new AsyncRelayCommand(cmd.ExecuteAsync), cmd.Gesture);
                view.InputBindings.Add(binding);
                _bindings.Add(binding);
            }
        }
    }

    private static IEnumerable<IUserCommand> ExtractCommands(object dataContext)
    {
        return dataContext.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => typeof(IUserCommand).IsAssignableFrom(p.PropertyType))
            .Select(p => p.GetValue(dataContext) as IUserCommand)
            .Where(c => c != null)!
            .Cast<IUserCommand>();
    }
}
