using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;

namespace Wrecept.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IInvoiceService _invoiceService;
    private readonly INavigationService _navigationService;

    public MainWindowViewModel() : this(WreceptAppContext.InvoiceService, WreceptAppContext.NavigationService)
    {
    }

    public MainWindowViewModel(IInvoiceService invoiceService, INavigationService navigationService)
    {
        _invoiceService = invoiceService;
        _navigationService = navigationService;
        _invoices = new ObservableCollection<Invoice>();
    }

    [ObservableProperty]
    private string _greeting = "Üdvözlet";

    [ObservableProperty]
    private ObservableCollection<Invoice> _invoices;

    [ObservableProperty]
    private Invoice? _selectedInvoice;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public void EnsureValidSelection()
    {
        if (SelectedInvoice is null && Invoices.Count > 0)
        {
            SelectedInvoice = Invoices[^1];
        }
    }

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
        Invoices = new ObservableCollection<Invoice>(result);
        SelectedInvoice = Invoices.FirstOrDefault();
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
    private void OpenInvoiceListView()
    {
        _navigationService.ShowInvoiceListView();
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
        _navigationService.ShowFilterByDateView(ApplyDateFilter);
        StatusMessage = "Dátum szűrő";
    }

    [RelayCommand]
    private void FilterBySupplierView()
    {
        _navigationService.ShowFilterBySupplierView(ApplySupplierFilter);
        StatusMessage = "Szállító szűrő";
    }

    [RelayCommand]
    private void FilterByProductGroupView()
    {
        _navigationService.ShowFilterByProductGroupView(ApplyProductGroupFilter);
        StatusMessage = "Termékcsoport szűrő";
    }

    [RelayCommand]
    private void FilterByProductView()
    {
        _navigationService.ShowFilterByProductView(ApplyProductFilter);
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
        // TODO: implement action
    }

    private async void ApplyDateFilter(DateOnly? from, DateOnly? to)
    {
        var result = await _invoiceService.GetByDateRange(from, to);
        Invoices = new ObservableCollection<Invoice>(result);
        SelectedInvoice = Invoices.FirstOrDefault();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }

    private async void ApplySupplierFilter(Guid? supplierId)
    {
        var result = supplierId.HasValue
            ? await _invoiceService.GetBySupplierId(supplierId.Value)
            : await _invoiceService.GetAllAsync();
        Invoices = new ObservableCollection<Invoice>(result);
        SelectedInvoice = Invoices.FirstOrDefault();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }

    private async void ApplyProductGroupFilter(Guid? groupId)
    {
        var result = groupId.HasValue
            ? await _invoiceService.GetByProductGroupId(groupId.Value)
            : await _invoiceService.GetAllAsync();
        Invoices = new ObservableCollection<Invoice>(result);
        SelectedInvoice = Invoices.FirstOrDefault();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }

    private async void ApplyProductFilter(Guid? productId)
    {
        var result = productId.HasValue
            ? await _invoiceService.GetByProductId(productId.Value)
            : await _invoiceService.GetAllAsync();
        Invoices = new ObservableCollection<Invoice>(result);
        SelectedInvoice = Invoices.FirstOrDefault();
        EnsureValidSelection();
        StatusMessage = "Szűrő alkalmazva";
    }
}
