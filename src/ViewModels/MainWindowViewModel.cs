using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.Core.Plugins;
using System.Collections.Generic;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;

namespace Wrecept.ViewModels;

public partial class MainWindowViewModel : RestorableListViewModel<Invoice>
{
    private readonly IInvoiceService _invoiceService;
    private readonly INavigationService _navigationService;

    public MainWindowViewModel() : this(WreceptAppContext.InvoiceService, WreceptAppContext.NavigationService, WreceptAppContext.MenuPlugins)
    {
    }

    public MainWindowViewModel(IInvoiceService invoiceService, INavigationService navigationService, IEnumerable<IMenuPlugin>? plugins = null)
    {
        _invoiceService = invoiceService;
        _navigationService = navigationService;
        _invoices = new ObservableCollection<Invoice>();
        PluginMenuItems = new ObservableCollection<PluginMenuItemViewModel>();
        plugins ??= Enumerable.Empty<IMenuPlugin>();
        foreach (var plugin in plugins)
            PluginMenuItems.Add(new PluginMenuItemViewModel(plugin));
    }

    [ObservableProperty]
    private string _greeting = "Üdvözlet";

    public ObservableCollection<PluginMenuItemViewModel> PluginMenuItems { get; }

    private ObservableCollection<Invoice> _invoices;

    protected override IList<Invoice> Items => _invoices;

    public ObservableCollection<Invoice> Invoices
    {
        get => _invoices;
        private set => SetProperty(ref _invoices, value);
    }

    public Invoice? SelectedInvoice
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public bool MoveSelectionUp()
    {
        if (Invoices.Count == 0)
            return false;

        if (SelectedInvoice is null)
        {
            SelectedInvoice = Invoices[0];
            return true;
        }

        var index = Invoices.IndexOf(SelectedInvoice);
        if (index <= 0)
            return false;

        SelectedInvoice = Invoices[index - 1];
        return true;
    }

    public bool MoveSelectionDown()
    {
        if (Invoices.Count == 0)
            return false;

        if (SelectedInvoice is null)
        {
            SelectedInvoice = Invoices[0];
            return true;
        }

        var index = Invoices.IndexOf(SelectedInvoice);
        if (index >= Invoices.Count - 1)
            return false;

        SelectedInvoice = Invoices[index + 1];
        return true;
    }

    [RelayCommand]
    public async Task LoadInvoicesAsync()
    {
        var result = await _invoiceService.GetAllAsync();
        var ordered = result.OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.Id);
        Invoices = new ObservableCollection<Invoice>(ordered);
        SelectedInvoice = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task AddInvoiceAsync()
    {
        var suppliers = await WreceptAppContext.SupplierService.GetAllAsync();
        var payments = await WreceptAppContext.PaymentMethodService.GetAllAsync();

        if (!suppliers.Any() || !payments.Any())
        {
            WreceptAppContext.SetStatus("Nincs rögzített szállító vagy fizetési mód.");
            return;
        }

        var invoice = new Invoice
        {
            SerialNumber = $"INV-{Invoices.Count + 1}",
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            Supplier = suppliers.First(),
            PaymentMethod = payments.First()
        };

        await _invoiceService.SaveAsync(invoice);
        Invoices.Add(invoice);
        SelectedInvoice = invoice;
        EnsureValidSelection();
    }

    private bool CanDeleteInvoice(Invoice? invoice) => invoice is not null;

    [RelayCommand(CanExecute = nameof(CanDeleteInvoice))]
    private async Task DeleteInvoiceAsync(Invoice? invoice)
    {
        if (invoice is null)
            return;

        await _invoiceService.DeleteAsync(invoice.Id);
        Invoices.Remove(invoice);
        if (ReferenceEquals(SelectedInvoice, invoice))
        {
            SelectedInvoice = null;
            EnsureValidSelection();
        }
    }

    [RelayCommand]
    private async Task OpenInvoiceListViewAsync()
    {
        await _navigationService.ShowInvoiceListViewAsync();
        StatusMessage = "Számlák kezelése";
    }

    [RelayCommand]
    private async Task RefreshInvoiceDataAsync()
    {
        await LoadInvoicesAsync();
        StatusMessage = "Számlák frissítve";
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
        _navigationService.ShowFilterByDateView(ApplyDateFilterAsync);
        StatusMessage = "Dátum szűrő";
    }

    [RelayCommand]
    private void FilterBySupplierView()
    {
        _navigationService.ShowFilterBySupplierView(ApplySupplierFilterAsync);
        StatusMessage = "Szállító szűrő";
    }

    [RelayCommand]
    private void FilterByProductGroupView()
    {
        _navigationService.ShowFilterByProductGroupView(ApplyProductGroupFilterAsync);
        StatusMessage = "Termékcsoport szűrő";
    }

    [RelayCommand]
    private void FilterByProductView()
    {
        _navigationService.ShowFilterByProductView(ApplyProductFilterAsync);
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

    [RelayCommand]
    private void ShowGreeting()
    {
        System.Windows.MessageBox.Show(Greeting, "Köszöntés");
    }

    private async Task ApplyDateFilterAsync(DateOnly? from, DateOnly? to)
    {
        var result = await _invoiceService.GetByDateRange(from, to);
        var ordered = result.OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.Id);
        Invoices = new ObservableCollection<Invoice>(ordered);
        SelectedInvoice = GetDefaultSelection();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }

    private async Task ApplySupplierFilterAsync(Guid? supplierId)
    {
        var result = supplierId.HasValue
            ? await _invoiceService.GetBySupplierId(supplierId.Value)
            : await _invoiceService.GetAllAsync();
        var orderedSupp = result.OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.Id);
        Invoices = new ObservableCollection<Invoice>(orderedSupp);
        SelectedInvoice = GetDefaultSelection();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }

    private async Task ApplyProductGroupFilterAsync(Guid? groupId)
    {
        var result = groupId.HasValue
            ? await _invoiceService.GetByProductGroupId(groupId.Value)
            : await _invoiceService.GetAllAsync();
        var orderedGroup = result.OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.Id);
        Invoices = new ObservableCollection<Invoice>(orderedGroup);
        SelectedInvoice = GetDefaultSelection();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }

    private async Task ApplyProductFilterAsync(Guid? productId)
    {
        var result = productId.HasValue
            ? await _invoiceService.GetByProductId(productId.Value)
            : await _invoiceService.GetAllAsync();
        var orderedProd = result.OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.Id);
        Invoices = new ObservableCollection<Invoice>(orderedProd);
        SelectedInvoice = GetDefaultSelection();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }
}
