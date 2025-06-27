using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Wrecept.ViewModels;

public partial class InvoiceSidebarViewModel : ObservableObject
{
    private readonly ObservableCollection<Invoice> _allInvoices;
    private readonly ISupplierService _supplierService;

    [ObservableProperty]
    private ObservableCollection<Invoice> _invoices;

    [ObservableProperty]
    private Invoice? _selectedInvoice;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private DateOnly? _fromDate;

    [ObservableProperty]
    private DateOnly? _toDate;

    [ObservableProperty]
    private ObservableCollection<Supplier> _suppliers = new();

    [ObservableProperty]
    private Supplier? _selectedSupplier;

    public InvoiceSidebarViewModel(ObservableCollection<Invoice> invoices, ISupplierService supplierService)
    {
        _supplierService = supplierService;
        _allInvoices = invoices;
        _invoices = new ObservableCollection<Invoice>(invoices);
        _selectedInvoice = invoices.FirstOrDefault();
        _ = LoadSuppliersAsync();
    }

    private async Task LoadSuppliersAsync()
    {
        var list = await _supplierService.GetAllAsync();
        Suppliers = new ObservableCollection<Supplier>(list);
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnFromDateChanged(DateOnly? value) => ApplyFilter();
    partial void OnToDateChanged(DateOnly? value) => ApplyFilter();
    partial void OnSelectedSupplierChanged(Supplier? value) => ApplyFilter();

    private void ApplyFilter()
    {
        var query = _allInvoices.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
            query = query.Where(i => i.SerialNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        if (FromDate.HasValue)
            query = query.Where(i => i.IssueDate >= FromDate.Value);
        if (ToDate.HasValue)
            query = query.Where(i => i.IssueDate <= ToDate.Value);
        if (SelectedSupplier != null)
            query = query.Where(i => i.Supplier.Id == SelectedSupplier.Id);
        Invoices = new ObservableCollection<Invoice>(query);
        if (SelectedInvoice == null || !Invoices.Contains(SelectedInvoice))
            SelectedInvoice = Invoices.FirstOrDefault();
    }
}
