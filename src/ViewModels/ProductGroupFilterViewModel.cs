using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class ProductGroupFilterViewModel : ObservableObject
{
    private readonly Action<Guid?> _apply;
    private readonly IProductGroupService _service;

    [ObservableProperty]
    private List<ProductGroup> _groups = new();

    [ObservableProperty]
    private ProductGroup? _selectedGroup;

    public ProductGroupFilterViewModel(Action<Guid?> apply, IProductGroupService service)
    {
        _apply = apply;
        _service = service;
        _groups = _service.GetAllAsync().Result;
    }

    [RelayCommand]
    private void Apply(object window)
    {
        _apply(SelectedGroup?.Id);
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
