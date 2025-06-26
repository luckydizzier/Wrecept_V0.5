using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wrecept.Core.Plugins;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public MainWindowViewModel() : this(Wrecept.Infrastructure.AppContext.NavigationService, Wrecept.Infrastructure.AppContext.MenuPlugins)
    {
    }

    public MainWindowViewModel(INavigationService navigationService, IEnumerable<IMenuPlugin>? plugins = null)
    {
        _navigationService = navigationService;
        PluginMenuItems = new ObservableCollection<PluginMenuItemViewModel>();
        plugins ??= Enumerable.Empty<IMenuPlugin>();
        foreach (var plugin in plugins)
            PluginMenuItems.Add(new PluginMenuItemViewModel(plugin));
    }

    [ObservableProperty]
    private string _greeting = "Üdvözlet";

    [ObservableProperty]
    private object? _currentView;

    public ObservableCollection<PluginMenuItemViewModel> PluginMenuItems { get; }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [RelayCommand]
    private async Task OpenInvoiceListViewAsync()
    {
        await _navigationService.ShowInvoiceListViewAsync();
        StatusMessage = "Számlák kezelése";
    }

    [RelayCommand]
    private void OpenSupplierView()
    {
        _navigationService.ShowSupplierView();
        StatusMessage = "Szállítók";
    }

    [RelayCommand]
    private void OpenProductView()
    {
        _navigationService.ShowProductView();
        StatusMessage = "Termékek";
    }

    [RelayCommand]
    private void FilterByDateView()
    {
        _navigationService.ShowFilterByDateView();
        StatusMessage = "Dátum szűrő";
    }

    [RelayCommand]
    private void FilterBySupplierView()
    {
        _navigationService.ShowFilterBySupplierView();
        StatusMessage = "Szállító szűrő";
    }

    [RelayCommand]
    private void FilterByProductGroupView()
    {
        _navigationService.ShowFilterByProductGroupView();
        StatusMessage = "Termékcsoport szűrő";
    }

    [RelayCommand]
    private void FilterByProductView()
    {
        _navigationService.ShowFilterByProductView();
        StatusMessage = "Termék szűrő";
    }

    [RelayCommand]
    private void OpenHelpView()
    {
        _navigationService.ShowHelpView();
        StatusMessage = "Súgó";
    }

    [RelayCommand]
    private void OpenAboutDialog()
    {
        _navigationService.ShowAboutDialog();
        StatusMessage = "Névjegy";
    }

    [RelayCommand]
    private void ShowOnboardingOverlay()
    {
        _navigationService.ShowOnboardingOverlay();
    }

    [RelayCommand]
    private void OpenSettingsView()
    {
        _navigationService.ShowSettingsView();
        StatusMessage = "Beállítások";
    }

    [RelayCommand]
    private void ExitApplication()
    {
        _navigationService.ExitApplication();
    }
}
