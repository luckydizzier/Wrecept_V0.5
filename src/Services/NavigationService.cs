using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
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
            var current = list.FirstOrDefault() ?? new Invoice { Supplier = new Supplier() };
            var vm = new InvoiceEditorViewModel(
                current,
                false,
                WreceptAppContext.InvoiceService,
                WreceptAppContext.SupplierService,
                WreceptAppContext.PaymentMethodService,
                WreceptAppContext.ProductService,
                WreceptAppContext.ProductGroupService,
                WreceptAppContext.UnitService,
                WreceptAppContext.TaxRateService,
                WreceptAppContext.PriceHistoryService,
                WreceptAppContext.FeedbackService,
                WreceptAppContext.DatabaseAvailable,
                list);
            var view = new Views.InvoiceEditorWindow { DataContext = vm };
            view.Loaded += (_, _) => vm.OnLoaded();
            Show(view);
        });
    }

    public void ShowSupplierView()
    {
        var vm = new SupplierListViewModel(
            WreceptAppContext.SupplierService,
            new StatusService { StatusMessageSetter = WreceptAppContext.StatusMessageSetter });
        var view = new Views.MasterData.SupplierView { DataContext = vm };
        Show(view);
    }

    public void ShowUnitView()
    {
        var vm = new UnitListViewModel(
            WreceptAppContext.UnitService,
            new StatusService { StatusMessageSetter = WreceptAppContext.StatusMessageSetter });
        var view = new Views.MasterData.UnitView { DataContext = vm };
        Show(view);
    }

    public void ShowProductGroupView()
    {
        var vm = new ProductGroupListViewModel(
            WreceptAppContext.ProductGroupService,
            new StatusService { StatusMessageSetter = WreceptAppContext.StatusMessageSetter });
        var view = new Views.MasterData.ProductGroupView { DataContext = vm };
        Show(view);
    }

    public void ShowTaxRateView()
    {
        var vm = new TaxRateListViewModel(
            WreceptAppContext.TaxRateService,
            new StatusService { StatusMessageSetter = WreceptAppContext.StatusMessageSetter });
        var view = new Views.MasterData.TaxRateView { DataContext = vm };
        Show(view);
    }

    public void ShowProductView()
    {
        var vm = new ProductListViewModel(
            WreceptAppContext.ProductService,
            WreceptAppContext.ProductGroupService,
            WreceptAppContext.TaxRateService,
            WreceptAppContext.UnitService,
            new StatusService { StatusMessageSetter = WreceptAppContext.StatusMessageSetter });
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
        var vm = new DateFilterViewModel(
            (_, _) => Task.CompletedTask,
            this);
        var dlg = new Views.Filters.DateFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterBySupplierView()
    {
        var vm = new SupplierFilterViewModel(
            _ => Task.CompletedTask,
            WreceptAppContext.SupplierService,
            this);
        var dlg = new Views.Filters.SupplierFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterByProductGroupView()
    {
        var vm = new ProductGroupFilterViewModel(
            _ => Task.CompletedTask,
            WreceptAppContext.ProductGroupService,
            this);
        var dlg = new Views.Filters.ProductGroupFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterByProductView()
    {
        var vm = new ProductFilterViewModel(
            _ => Task.CompletedTask,
            WreceptAppContext.ProductService,
            this);
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
