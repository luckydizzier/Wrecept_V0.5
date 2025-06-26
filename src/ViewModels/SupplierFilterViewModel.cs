using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class SupplierFilterViewModel : ObservableObject
{
    private readonly Func<Guid?, Task> _apply;
    private readonly ISupplierService _service;

    [ObservableProperty]
    private List<Supplier> _suppliers = new();

    [ObservableProperty]
    private Supplier? _selectedSupplier;

    public SupplierFilterViewModel(Func<Guid?, Task> apply, ISupplierService service)
    {
        _apply = apply;
        _service = service;
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
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }
}
