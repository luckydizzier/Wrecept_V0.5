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

public partial class SupplierFilterViewModel : ObservableObject
{
    private readonly Func<Guid?, Task> _apply;
    private readonly ISupplierService _service;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private List<Supplier> _suppliers = new();

    [ObservableProperty]
    private Supplier? _selectedSupplier;

    public SupplierFilterViewModel(Func<Guid?, Task> apply, ISupplierService service, INavigationService navigation)
    {
        _apply = apply;
        _service = service;
        _navigation = navigation;
        _ = LoadSuppliersAsync();
    }

    private async Task LoadSuppliersAsync()
    {
        Suppliers = await _service.GetAllAsync();
        SelectedSupplier = Suppliers.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ApplyAsync()
    {
        await _apply(SelectedSupplier?.Id);
        _navigation.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigation.CloseCurrentView();
    }
}
