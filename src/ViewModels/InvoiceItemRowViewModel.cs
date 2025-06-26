using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public partial class InvoiceItemRowViewModel : ObservableObject
{
    public bool IsPlaceholder { get; init; }

    [ObservableProperty]
    private string _productName = string.Empty;

    [ObservableProperty]
    private decimal _quantity;

    [ObservableProperty]
    private string _unitName = string.Empty;

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
        Id = Guid.NewGuid(),
        Product = new Product { Name = ProductName },
        Quantity = Quantity,
        Unit = new Unit { Name = UnitName },
        UnitPriceNet = UnitPriceNet,
        VatRatePercent = VatRatePercent
    };

    public void Clear()
    {
        ProductName = string.Empty;
        Quantity = 0;
        UnitName = string.Empty;
        UnitPriceNet = 0m;
        VatRatePercent = 0m;
    }
}
