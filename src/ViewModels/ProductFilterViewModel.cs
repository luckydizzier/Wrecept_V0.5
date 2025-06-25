using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class ProductFilterViewModel : ObservableObject
{
    private readonly Action<Guid?> _apply;
    private readonly IProductService _service;

    [ObservableProperty]
    private List<Product> _products = new();

    [ObservableProperty]
    private Product? _selectedProduct;

    public ProductFilterViewModel(Action<Guid?> apply, IProductService service)
    {
        _apply = apply;
        _service = service;
        _products = _service.GetAllAsync().Result;
    }

    [RelayCommand]
    private void Apply(object window)
    {
        _apply(SelectedProduct?.Id);
        if (window is System.Windows.Window w)
            w.DialogResult = true;
    }

    [RelayCommand]
    private void Cancel(object window)
    {
        if (window is System.Windows.Window w)
            w.DialogResult = false;
    }
}
