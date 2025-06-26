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
    private async Task ApplyAsync(object window)
    {
        await _apply(SelectedSupplier?.Id);
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
