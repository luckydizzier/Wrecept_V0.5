using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class SupplierListViewModel : RestorableListViewModel<Supplier>
{
    private readonly ISupplierService _service;

    private ObservableCollection<Supplier> _suppliers = new();

    protected override IList<Supplier> Items => _suppliers;

    public ObservableCollection<Supplier> Suppliers
    {
        get => _suppliers;
        private set => SetProperty(ref _suppliers, value);
    }

    public Supplier? SelectedSupplier
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    public SupplierListViewModel(ISupplierService service)
    {
        _service = service;
        var items = _service.GetAllAsync().Result;
        var ordered = items.OrderByDescending(s => s.Id);
        _suppliers = new ObservableCollection<Supplier>(ordered);
        SelectedSupplier = GetDefaultSelection();
    }

    [RelayCommand]
    private void Add()
    {
        var supplier = new Supplier { Id = Guid.NewGuid(), Name = "" };
        _suppliers.Add(supplier);
        SelectedSupplier = supplier;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedSupplier is null) return;
        await _service.SaveAsync(SelectedSupplier);
        Infrastructure.AppContext.SetStatus("Beszállító mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedSupplier is null) return;
        await _service.DeleteAsync(SelectedSupplier.Id);
        _suppliers.Remove(SelectedSupplier);
    }
}
