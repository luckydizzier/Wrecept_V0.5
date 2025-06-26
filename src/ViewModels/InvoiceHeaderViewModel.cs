using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Views.Lookup;
using Wrecept.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wrecept.ViewModels;

public partial class InvoiceHeaderViewModel : ObservableObject
{
    public Invoice Invoice { get; }

    [ObservableProperty]
    private List<PaymentMethod> _paymentMethods = new();

    public IReadOnlyList<CalculationMode> CalculationModes { get; }

    public InlineSupplierCreatorViewModel? SupplierCreator { get; private set; }

    private readonly ISupplierService _supplierService;
    private readonly ILookupDialogPresenter _lookupPresenter;
    private readonly IPaymentMethodService _paymentMethodService;

    public InvoiceHeaderViewModel(
        Invoice invoice,
        IPaymentMethodService paymentMethodService,
        ISupplierService supplierService,
        ILookupDialogPresenter lookupPresenter)
    {
        _supplierService = supplierService;
        _lookupPresenter = lookupPresenter;
        _paymentMethodService = paymentMethodService;
        Invoice = invoice;
        CalculationModes = new[] { CalculationMode.Net, CalculationMode.Gross };
        _ = Task.Run(async () =>
        {
            try
            {
                await LoadPaymentMethodsAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Infrastructure.AppContext.SetStatus(ex.Message);
            }
        });
    }

    private async Task LoadPaymentMethodsAsync()
    {
        PaymentMethods = await _paymentMethodService.GetAllAsync();
        if (Invoice.PaymentMethod == null && PaymentMethods.Count > 0)
            Invoice.PaymentMethod = PaymentMethods[0];
    }

    public async Task<bool> TryOpenSupplierCreatorAsync()
    {
        var name = Invoice.Supplier.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var existingList = await _supplierService.GetAllAsync();
        var existing = existingList
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

    public async Task<bool> OpenSupplierLookupAsync()
    {
        async Task<List<Supplier>> Search(string term)
        {
            var all = await _supplierService.GetAllAsync();
            return all.Where(s => s.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var vm = new LookupDialogViewModel<Supplier>(Search, s => s.Name);
        var result = _lookupPresenter.ShowDialog(vm);
        if (result == true && vm.SelectedItem != null)
        {
            Invoice.Supplier = vm.SelectedItem.Value;
            OnPropertyChanged(nameof(Invoice));
            return true;
        }
        return false;
    }
}
