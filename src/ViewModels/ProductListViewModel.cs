using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        _products = new ObservableCollection<Product>(_service.GetAllAsync().Result);
    }

    [RelayCommand]
    private void Add()
    {
        var product = new Product { Id = Guid.NewGuid(), Name = string.Empty, Group = new ProductGroup(), TaxRate = new TaxRate(), DefaultUnit = new Unit() };
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
