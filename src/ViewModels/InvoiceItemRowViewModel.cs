using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public partial class InvoiceItemRowViewModel : ObservableObject
{
    public bool IsPlaceholder { get; init; }

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _productName = string.Empty;

    partial void OnProductNameChanged(string value) => AutoValidate();

    [ObservableProperty]
    private decimal _quantity;

    partial void OnQuantityChanged(decimal value) => AutoValidate();

    [ObservableProperty]
    private string _unitName = string.Empty;

    partial void OnUnitNameChanged(string value) => AutoValidate();

    [ObservableProperty]
    private decimal _unitPriceNet;

    [ObservableProperty]
    private decimal _vatRatePercent;

    public decimal Net => Quantity * UnitPriceNet;
    public decimal Vat => Net * VatRatePercent / 100m;
    public decimal Gross => Net + Vat;

    public InvoiceItemRowViewModel()
    {
    }

    public InvoiceItemRowViewModel(InvoiceItem item)
    {
        _productName = item.Product.Name;
        _quantity = item.Quantity;
        _unitName = item.Unit.Name;
        _unitPriceNet = item.UnitPriceNet;
        _vatRatePercent = item.VatRatePercent;
    }

    public InvoiceItem ToModel() => new()
    {
        Id = Guid.Empty,
        Product = new Product { Name = ProductName },
        Quantity = Quantity,
        Unit = new Unit { Name = UnitName },
        UnitPriceNet = UnitPriceNet,
        VatRatePercent = VatRatePercent
    };

    private void AutoValidate()
    {
        if (IsPlaceholder)
            Validate();
    }

    public void Clear()
    {
        ProductName = string.Empty;
        Quantity = 0;
        UnitName = string.Empty;
        UnitPriceNet = 0m;
        VatRatePercent = 0m;
        HasError = false;
    }

    public bool Validate()
    {
        HasError = string.IsNullOrWhiteSpace(ProductName)
                   || Quantity <= 0
                   || string.IsNullOrWhiteSpace(UnitName)
                   || UnitPriceNet <= 0m;
        return !HasError;
    }
}
