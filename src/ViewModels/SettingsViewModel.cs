using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Infrastructure;
using Wrecept;

namespace Wrecept.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _theme = "Light";

    public SettingsViewModel()
    {
        var settings = SettingsService.Load();
        _theme = settings.Theme;
    }

    [RelayCommand]
    private void Save(object window)
    {
        SettingsService.Save(new Settings { Theme = Theme });
        App.ApplyTheme(Theme);
        if (window is System.Windows.Window w)
            w.DialogResult = true;
    }

    [RelayCommand]
    private void Cancel(object window)
    {
        if (window is System.Windows.Window w)
            w.DialogResult = false;
    }
}
