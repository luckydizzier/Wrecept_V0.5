using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Infrastructure;
using Wrecept.Services;
using Wrecept;

namespace Wrecept.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _service;

    [ObservableProperty]
    private string _theme = "Light";

    [ObservableProperty]
    private string _language = "hu";


    public SettingsViewModel(ISettingsService service)
    {
        _service = service;
        var settings = _service.LoadAsync().GetAwaiter().GetResult();
        _theme = settings.Theme;
        _language = settings.Language;
    }

    [RelayCommand]
    private async Task SaveAsync(object window)
    {
        await _service.SaveAsync(new Settings { Theme = Theme, Language = Language });
        App.ApplyTheme(Theme);
        App.ApplyLanguage(Language);
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
