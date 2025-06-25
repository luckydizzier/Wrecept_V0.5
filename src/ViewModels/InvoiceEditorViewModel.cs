using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class InvoiceEditorViewModel : ObservableObject
{
    private readonly Invoice _original;
    private readonly IInvoiceService _invoiceService;

    public InvoiceSidebarViewModel SidebarViewModel { get; }
    public InvoiceHeaderViewModel HeaderViewModel { get; }
    public InvoiceItemsViewModel ItemsViewModel { get; }
    public InvoiceSummaryViewModel SummaryViewModel { get; }

    public ObservableCollection<VatSummary> VatSummaries { get; } = new();
    public GrandTotal GrandTotals { get; private set; } = new(0m, 0m);

    public IAsyncRelayCommand SaveCommand { get; }
    public IRelayCommand ExitToListCommand { get; }
    public bool ExitRequested { get; private set; }
    [ObservableProperty]
    private Invoice _invoice;

    [ObservableProperty]
    private bool _isEditMode;

    public bool IsReadOnly => !IsEditMode;

    public InvoiceEditorViewModel(Invoice invoice, bool isEditMode, IInvoiceService invoiceService)
    {
        _original = invoice;
        _invoiceService = invoiceService;
        _invoice = new Invoice
        {
            Id = invoice.Id,
            SerialNumber = invoice.SerialNumber,
            IssueDate = invoice.IssueDate
        };
        IsEditMode = isEditMode;
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        ExitToListCommand = new RelayCommand(() => ExitRequested = true);

        SidebarViewModel = new InvoiceSidebarViewModel(new ObservableCollection<Invoice>());
        HeaderViewModel = new InvoiceHeaderViewModel(Invoice, Array.Empty<string>(), Array.Empty<string>());
        ItemsViewModel = new InvoiceItemsViewModel(Invoice);
        SummaryViewModel = new InvoiceSummaryViewModel(VatSummaries, GrandTotals);
    }

    public void CancelEdit()
    {
        Invoice = new Invoice
        {
            Id = _original.Id,
            SerialNumber = _original.SerialNumber,
            IssueDate = _original.IssueDate
        };
    }

    public async Task SaveAsync()
    {
        await _invoiceService.SaveAsync(Invoice);
        ExitRequested = true;
    }

    public void OnLoaded()
    {
        UpdateSummaries();
    }

    private void UpdateSummaries()
    {
        VatSummaries.Clear();
        var groups = Invoice.Items.GroupBy(i => i.VatRatePercent);
        foreach (var g in groups)
        {
            var net = g.Sum(it => it.UnitPriceNet * it.Quantity);
            var vat = net * (decimal)g.Key / 100m;
            VatSummaries.Add(new VatSummary((decimal)g.Key, net, vat));
        }
        var totalNet = VatSummaries.Sum(v => v.Net);
        var totalVat = VatSummaries.Sum(v => v.Vat);
        GrandTotals = new GrandTotal(totalNet, totalVat);
        OnPropertyChanged(nameof(GrandTotals));
    }

    public record VatSummary(decimal Rate, decimal Net, decimal Vat)
    {
        public decimal Gross => Net + Vat;
    }

    public record GrandTotal(decimal Net, decimal Vat)
    {
        public decimal Gross => Net + Vat;
        public string AmountText => string.Empty;
    }
}

