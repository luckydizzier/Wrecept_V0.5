using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;

namespace Wrecept.Services;

public class NavigationService : INavigationService
{
    private readonly IInvoiceService _invoiceService;
    private readonly ISupplierService _supplierService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IProductService _productService;
    private readonly IProductGroupService _productGroupService;
    private readonly IUnitService _unitService;
    private readonly ITaxRateService _taxRateService;
    private readonly IPriceHistoryService _priceHistoryService;
    private readonly IFeedbackService _feedbackService;
    private readonly ISettingsService _settingsService;
    private MainWindowViewModel? _host;

    public NavigationService() : this(
        new DefaultInvoiceService(new InMemoryInvoiceRepository()),
        new DefaultSupplierService(new InMemorySupplierRepository()),
        new DefaultPaymentMethodService(new InMemoryPaymentMethodRepository()),
        new DefaultProductService(new InMemoryProductRepository()),
        new DefaultProductGroupService(new InMemoryProductGroupRepository()),
        new DefaultUnitService(new InMemoryUnitRepository()),
        new DefaultTaxRateService(new InMemoryTaxRateRepository()),
        new JsonPriceHistoryService(),
        new FeedbackService(),
        new JsonSettingsService())
    {
    }

    public NavigationService(
        IInvoiceService invoiceService,
        ISupplierService supplierService,
        IPaymentMethodService paymentMethodService,
        IProductService productService,
        IProductGroupService productGroupService,
        IUnitService unitService,
        ITaxRateService taxRateService,
        IPriceHistoryService priceHistoryService,
        IFeedbackService feedbackService,
        ISettingsService settingsService)
    {
        _invoiceService = invoiceService;
        _supplierService = supplierService;
        _paymentMethodService = paymentMethodService;
        _productService = productService;
        _productGroupService = productGroupService;
        _unitService = unitService;
        _taxRateService = taxRateService;
        _priceHistoryService = priceHistoryService;
        _feedbackService = feedbackService;
        _settingsService = settingsService;
    }

    public void SetHost(MainWindowViewModel host) => _host = host;

    private void Show(UserControl view)
    {
        if (_host is not null)
            _host.CurrentView = view;
    }

    public async Task ShowInvoiceListViewAsync()
    {
        var invoices = await _invoiceService.GetAllAsync();
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var list = new ObservableCollection<Invoice>(invoices);
            var current = list.FirstOrDefault() ?? new Invoice { Supplier = new Supplier() };
            var vm = new InvoiceEditorViewModel(
                current,
                false,
                _invoiceService,
                _supplierService,
                _paymentMethodService,
                _productService,
                _productGroupService,
                _unitService,
                _taxRateService,
                _priceHistoryService,
                _feedbackService,
                App.Services.GetRequiredService<IKeyboardDialogService>(),
                this,
                Infrastructure.AppContext.DatabaseAvailable,
                list);
            var view = new Views.InvoiceEditorWindow { DataContext = vm };
            view.Loaded += (_, _) => vm.OnLoaded();
            Show(view);
        });
    }

    public void ShowSupplierView()
    {
        var vm = new SupplierListViewModel(
            _supplierService,
            new StatusService { StatusMessageSetter = Infrastructure.AppContext.StatusMessageSetter });
        var view = new Views.MasterData.SupplierView { DataContext = vm };
        Show(view);
    }

    public void ShowUnitView()
    {
        var vm = new UnitListViewModel(
            _unitService,
            new StatusService { StatusMessageSetter = Infrastructure.AppContext.StatusMessageSetter });
        var view = new Views.MasterData.UnitView { DataContext = vm };
        Show(view);
    }

    public void ShowProductGroupView()
    {
        var vm = new ProductGroupListViewModel(
            _productGroupService,
            new StatusService { StatusMessageSetter = Infrastructure.AppContext.StatusMessageSetter });
        var view = new Views.MasterData.ProductGroupView { DataContext = vm };
        Show(view);
    }

    public void ShowTaxRateView()
    {
        var vm = new TaxRateListViewModel(
            _taxRateService,
            new StatusService { StatusMessageSetter = Infrastructure.AppContext.StatusMessageSetter });
        var view = new Views.MasterData.TaxRateView { DataContext = vm };
        Show(view);
    }

    public void ShowProductView()
    {
        var vm = new ProductListViewModel(
            _productService,
            _productGroupService,
            _taxRateService,
            _unitService,
            new StatusService { StatusMessageSetter = Infrastructure.AppContext.StatusMessageSetter });
        var view = new Views.MasterData.ProductView { DataContext = vm };
        Show(view);
    }

    public void ShowSettingsView()
    {
        var vm = new SettingsViewModel(_settingsService, this);
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
            _supplierService,
            this);
        var dlg = new Views.Filters.SupplierFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterByProductGroupView()
    {
        var vm = new ProductGroupFilterViewModel(
            _ => Task.CompletedTask,
            _productGroupService,
            this);
        var dlg = new Views.Filters.ProductGroupFilterDialog { DataContext = vm };
        Show(dlg);
    }

    public void ShowFilterByProductView()
    {
        var vm = new ProductFilterViewModel(
            _ => Task.CompletedTask,
            _productService,
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

    public void ShowSavingOverlay()
    {
        var overlay = new Views.SavingOverlay();
        Show(overlay);
    }

    public void CloseCurrentView()
    {
        if (_host is not null)
            _host.CurrentView = null;
    }

    public void ExitApplication()
    {
        _feedbackService.Exit();
        System.Windows.Application.Current.Shutdown();
    }
}
