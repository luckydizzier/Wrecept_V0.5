using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class InlineSupplierCreatorViewModel : InlineCreatorViewModel<Supplier>
{
    private readonly ISupplierService _supplierService;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _taxId = string.Empty;

    public InlineSupplierCreatorViewModel(ISupplierService supplierService, string name)
    {
        _supplierService = supplierService;
        _name = name;
    }

    protected override async Task<Supplier> CreateEntityAsync()
    {
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = Name,
            TaxId = TaxId
        };
        await _supplierService.SaveAsync(supplier);
        return supplier;
    }
}
