using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wrecept.Core.Domain;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;
using Wrecept.ViewModels;

namespace Wrecept.Services;

public class NavigationService : INavigationService
{
    private MainWindowViewModel? _host;

    public void SetHost(MainWindowViewModel host) => _host = host;

    private void Show(UserControl view)
    {
        if (_host is not null)
            _host.CurrentView = view;
    }

    public async Task ShowInvoiceListViewAsync()
    {
        var invoices = await WreceptAppContext.InvoiceService.GetAllAsync();
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var list = new ObservableCollection<Invoice>(invoices);
            var current = list.FirstOrDefault() ?? new Invoice();
            var vm = new InvoiceEditorViewModel(current, false, WreceptAppContext.InvoiceService, list);
            var view = new Views.InvoiceEditorWindow { DataContext = vm };
            view.Loaded += (_, _) => vm.OnLoaded();
            Show(view);
        });
    }

    public void ShowSupplierView()
    {
        var vm = new SupplierListViewModel(WreceptAppContext.SupplierService);
        var view = new Views.MasterData.SupplierView { DataContext = vm };
        Show(view);
    }

    public void ShowProductView()
    {
        var vm = new ProductListViewModel(WreceptAppContext.ProductService);
        var view = new Views.MasterData.ProductView { DataContext = vm };
        Show(view);
    }

    public void ShowSettingsView()
    {
        var vm = new SettingsViewModel(WreceptAppContext.SettingsService);
        var view = new Views.Settings.SettingsWindow { DataContext = vm };
        Show(view);
    }

    public void ShowFilterByDateView()
    {
        var vm = new DateFilterViewModel(async (_, _) => { });
        var dlg = new Views.Filters.DateFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterBySupplierView()
    {
        var vm = new SupplierFilterViewModel(async _ => { }, WreceptAppContext.SupplierService);
        var dlg = new Views.Filters.SupplierFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterByProductGroupView()
    {
        var vm = new ProductGroupFilterViewModel(async _ => { }, WreceptAppContext.ProductGroupService);
        var dlg = new Views.Filters.ProductGroupFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterByProductView()
    {
        var vm = new ProductFilterViewModel(async _ => { }, WreceptAppContext.ProductService);
        var dlg = new Views.Filters.ProductFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowHelpView()
    {
        var view = new Views.Help.HelpWindow();
        Show(view);
    }

    public void ShowAboutDialog()
    {
        var view = new Views.Help.AboutWindow();
        Show(view);
    }

    public void ShowOnboardingOverlay()
    {
        var overlay = new Views.OnboardingOverlay();
        Show(overlay);
    }

    public void CloseCurrentView()
    {
        if (_host is not null)
            _host.CurrentView = null;
    }

    public void ExitApplication()
    {
        WreceptAppContext.FeedbackService.Exit();
        System.Windows.Application.Current.Shutdown();
    }
}
