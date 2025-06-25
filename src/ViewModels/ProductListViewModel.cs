using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Wrecept.Infrastructure;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly IProductService _service;

    [ObservableProperty]
    private ObservableCollection<Product> _products = new();

    [ObservableProperty]
    private Product? _selectedProduct;

    public ProductListViewModel(IProductService service)
    {
        _service = service;
        var items = _service.GetAllAsync().Result;
        var ordered = items.OrderByDescending(p => p.Id);
        _products = new ObservableCollection<Product>(ordered);
        _selectedProduct = _products.FirstOrDefault();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var group = (await Infrastructure.AppContext.ProductGroupService.GetAllAsync()).First();
        var tax = (await Infrastructure.AppContext.TaxRateService.GetAllAsync()).First();
        var unit = (await Infrastructure.AppContext.UnitService.GetAllAsync()).First();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Group = group,
            TaxRate = tax,
            DefaultUnit = unit
        };
        _products.Add(product);
        SelectedProduct = product;
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
    }
}
