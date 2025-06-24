using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;

namespace Wrecept.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IInvoiceService _invoiceService;

    public MainWindowViewModel() : this(WreceptAppContext.InvoiceService)
    {
    }

    public MainWindowViewModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        _invoices = new ObservableCollection<Invoice>();
    }

    [ObservableProperty]
    private string _greeting = "Üdvözlet";

    [ObservableProperty]
    private ObservableCollection<Invoice> _invoices;

    [ObservableProperty]
    private Invoice? _selectedInvoice;

    [RelayCommand]
    public async Task LoadInvoicesAsync()
    {
        var result = await _invoiceService.GetAllAsync();
        Invoices = new ObservableCollection<Invoice>(result);
        SelectedInvoice = Invoices.FirstOrDefault();
    }

    [RelayCommand]
    private async Task AddInvoiceAsync()
    {
        var invoice = new Invoice { SerialNumber = $"INV-{Invoices.Count + 1}" };
        await _invoiceService.SaveAsync(invoice);
        Invoices.Add(invoice);
    }

    [RelayCommand]
    private async Task DeleteInvoiceAsync(Invoice invoice)
    {
        await _invoiceService.DeleteAsync(invoice.Id);
        Invoices.Remove(invoice);
    }

    [RelayCommand]
    private void ShowGreeting()
    {
        // TODO: implement action
    }
}
