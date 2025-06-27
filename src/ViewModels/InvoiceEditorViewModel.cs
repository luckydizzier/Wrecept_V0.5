using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
    public IRelayCommand PrintCommand { get; }
    public IRelayCommand ExportCommand { get; }
    public IRelayCommand ExitToListCommand { get; }
    public IRelayCommand CancelByEscCommand { get; }
    public bool ExitRequested { get; private set; }
    public bool ExitedByEsc { get; private set; }
    [ObservableProperty]
    private Invoice _invoice;

    [ObservableProperty]
    private bool _isEditMode;

    public bool IsReadOnly => !IsEditMode;
    public bool IsDatabaseAvailable => Infrastructure.AppContext.DatabaseAvailable;

    public InvoiceEditorViewModel(Invoice invoice, bool isEditMode, IInvoiceService invoiceService, ObservableCollection<Invoice>? invoices = null)
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
        PrintCommand = new RelayCommand(PrintInvoice);
        ExportCommand = new RelayCommand(ExportInvoice);
        ExitToListCommand = new RelayCommand(() => ExitRequested = true);
        CancelByEscCommand = new RelayCommand(CancelByEsc);

        SidebarViewModel = new InvoiceSidebarViewModel(
            invoices ?? new ObservableCollection<Invoice>(),
            Infrastructure.AppContext.SupplierService);
        HeaderViewModel = new InvoiceHeaderViewModel(
            Invoice,
            Infrastructure.AppContext.PaymentMethodService,
            Infrastructure.AppContext.SupplierService,
            Infrastructure.AppContext.LookupPresenter);
        ItemsViewModel = new InvoiceItemsViewModel(
            Invoice,
            Infrastructure.AppContext.ProductService,
            Infrastructure.AppContext.ProductGroupService,
            Infrastructure.AppContext.UnitService,
            Infrastructure.AppContext.TaxRateService,
            Infrastructure.AppContext.LookupPresenter);
        SummaryViewModel = new InvoiceSummaryViewModel(VatSummaries, GrandTotals);
    }

    public void CancelEdit()
    {
        Invoice.Id = _original.Id;
        Invoice.SerialNumber = _original.SerialNumber;
        Invoice.IssueDate = _original.IssueDate;
        Invoice.Supplier = _original.Supplier;
        Invoice.PaymentMethod = _original.PaymentMethod;
        Invoice.Notes = _original.Notes;
        Invoice.Items.Clear();
        foreach (var item in _original.Items)
        {
            Invoice.Items.Add(new InvoiceItem
            {
                Id = item.Id,
                Product = item.Product,
                Quantity = item.Quantity,
                Unit = item.Unit,
                UnitPriceNet = item.UnitPriceNet,
                VatRatePercent = item.VatRatePercent
            });
        }
        ItemsViewModel.Rows.Clear();
        ItemsViewModel.Rows.Add(ItemsViewModel.Entry);
        foreach (var item in Invoice.Items)
        {
            ItemsViewModel.Rows.Add(new InvoiceItemRowViewModel(item));
        }
        HeaderViewModel.NotifyInvoiceChanged();
        UpdateSummaries();
    }

    public void CancelByEsc()
    {
        CancelEdit();
        ExitedByEsc = true;
        ExitRequested = true;
    }

    public async Task SaveAsync()
    {
        await _invoiceService.SaveAsync(Invoice);
        foreach (var item in Invoice.Items)
        {
            Infrastructure.AppContext.PriceHistoryService.RecordPrice(item.Product.Name, item.UnitPriceNet);
        }
        ExitRequested = true;
        ExitedByEsc = false;
    }

    private void PrintInvoice()
    {
        var dlg = new System.Windows.Controls.PrintDialog();
        var doc = new System.Windows.Documents.FlowDocument(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run($"Számla: {Invoice.SerialNumber}")))
        {
            PageHeight = dlg.PrintableAreaHeight,
            PageWidth = dlg.PrintableAreaWidth
        };
        doc.ColumnWidth = dlg.PrintableAreaWidth;
        if (dlg.ShowDialog() == true)
        {
            dlg.PrintDocument(((System.Windows.Documents.IDocumentPaginatorSource)doc).DocumentPaginator, "Invoice");
        }
    }

    private void ExportInvoice()
    {
        var dlg = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "JSON (*.json)|*.json",
            FileName = $"invoice_{Invoice.SerialNumber}.json"
        };
        if (dlg.ShowDialog() == true)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Invoice);
            try
            {
                System.IO.File.WriteAllText(dlg.FileName, json);
            }
            catch (Exception ex)
            {
                LogError(ex);
                System.Windows.MessageBox.Show(
                    "A számla exportálása nem sikerült. Részletek az errors.log-ban.",
                    "Export hiba", System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }
    }

    private static void LogError(Exception ex)
    {
        var dir = Infrastructure.AppDirectories.GetWritableAppDataDirectory();
        var logPath = System.IO.Path.Combine(dir, "errors.log");
        var line = $"{System.DateTime.UtcNow:o} | {ex}\n";
        System.IO.File.AppendAllText(logPath, line);
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
        public string AmountText =>
            Wrecept.Core.Utilities.HungarianNumberConverter.ToText(Gross);
    }
}

