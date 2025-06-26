using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public class InvoiceHeaderViewModel : ObservableObject
{
    public Invoice Invoice { get; }
    public IEnumerable<string> PaymentMethods { get; }
    public IEnumerable<string> CalculationModes { get; }
    public InlineSupplierCreatorViewModel? SupplierCreator { get; private set; }

    private readonly ISupplierService _supplierService;

    public InvoiceHeaderViewModel(
        Invoice invoice,
        IEnumerable<string> paymentMethods,
        IEnumerable<string> calculationModes,
        ISupplierService supplierService)
    {
        _supplierService = supplierService;
        Invoice = invoice;
        PaymentMethods = paymentMethods;
        CalculationModes = calculationModes;
    }

    public bool TryOpenSupplierCreator()
    {
        var name = Invoice.Supplier.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var existing = _supplierService.GetAllAsync().Result
            .Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (existing)
            return false;

        SupplierCreator = new InlineSupplierCreatorViewModel(_supplierService, name);
        SupplierCreator.Saved += OnSupplierSaved;
        SupplierCreator.Canceled += OnSupplierCanceled;
        OnPropertyChanged(nameof(SupplierCreator));
        return true;
    }

    private void OnSupplierSaved(Supplier supplier)
    {
        Invoice.Supplier = supplier;
        SupplierCreator = null;
        OnPropertyChanged(nameof(SupplierCreator));
    }

    private void OnSupplierCanceled()
    {
        SupplierCreator = null;
        OnPropertyChanged(nameof(SupplierCreator));
    }
}
