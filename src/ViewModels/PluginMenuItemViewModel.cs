using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Plugins;

namespace Wrecept.ViewModels;

public class PluginMenuItemViewModel
{
    public string Header { get; }
    public ICommand Command { get; }

    public PluginMenuItemViewModel(IMenuPlugin plugin)
    {
        Header = plugin.MenuHeader;
        Command = new RelayCommand(plugin.Execute);
    }
}
