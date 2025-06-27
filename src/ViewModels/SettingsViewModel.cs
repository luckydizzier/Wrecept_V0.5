using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Services;
using Wrecept.Infrastructure;
using Wrecept;
using System.Threading.Tasks;

namespace Wrecept.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _service;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string _theme = "Light";

    [ObservableProperty]
    private string _language = "hu";


    public SettingsViewModel(ISettingsService service, INavigationService navigation)
    {
        _service = service;
        _navigation = navigation;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var settings = await _service.LoadAsync();
        Theme = settings.Theme;
        Language = settings.Language;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _service.SaveAsync(new Settings { Theme = Theme, Language = Language });
        App.ApplyTheme(Theme);
        App.ApplyLanguage(Language);
        _navigation.CloseCurrentView();
    }


    [RelayCommand]
    private void Cancel()
    {
        _navigation.CloseCurrentView();
    }
}
