using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Infrastructure;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class ProductListViewModel : RestorableListViewModel<Product>
{
    private readonly IProductService _service;

    private ObservableCollection<Product> _products = new();


    protected override IList<Product> Items => _products;

    public ObservableCollection<Product> Products
    {
        get => _products;
        private set => SetProperty(ref _products, value);
    }

    public Product? SelectedProduct
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    public ProductListViewModel(IProductService service)
    {
        _service = service;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        var ordered = items.OrderByDescending(p => p.Id);
        Products = new ObservableCollection<Product>(ordered);
        SelectedProduct = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var groups = await Infrastructure.AppContext.ProductGroupService.GetAllAsync();
        var taxes = await Infrastructure.AppContext.TaxRateService.GetAllAsync();
        var units = await Infrastructure.AppContext.UnitService.GetAllAsync();

        if (!groups.Any() || !taxes.Any() || !units.Any())
        {
            Infrastructure.AppContext.SetStatus("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.");
            return;
        }

        var product = new Product
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Group = groups.First(),
            TaxRate = taxes.First(),
            DefaultUnit = units.First()
        };

        _products.Add(product);
        SelectedProduct = product;
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedProduct is null) return;
        await _service.SaveAsync(SelectedProduct);
        Infrastructure.AppContext.SetStatus("Termék mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedProduct is null) return;
        await _service.DeleteAsync(SelectedProduct.Id);
        _products.Remove(SelectedProduct);
        SelectedProduct = null;
        EnsureValidSelection();
    }
}
