using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class ProductFilterViewModel : ObservableObject
{
    private readonly Func<Guid?, Task> _apply;
    private readonly IProductService _service;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private List<Product> _products = new();

    [ObservableProperty]
    private Product? _selectedProduct;

    public ProductFilterViewModel(Func<Guid?, Task> apply, IProductService service, INavigationService navigation)
    {
        _apply = apply;
        _service = service;
        _navigation = navigation;
        _ = LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        Products = await _service.GetAllAsync();
        SelectedProduct = Products.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ApplyAsync()
    {
        await _apply(SelectedProduct?.Id);
        _navigation.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigation.CloseCurrentView();
    }
}
