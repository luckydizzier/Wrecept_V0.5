using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class SupplierListViewModel : RestorableListViewModel<Supplier>
{
    private readonly ISupplierService _service;
    private readonly IStatusService _statusService;

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

    public SupplierListViewModel(ISupplierService service, IStatusService statusService)
    {
        _service = service;
        _statusService = statusService;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        var ordered = items.OrderByDescending(s => s.Id);
        Suppliers = new ObservableCollection<Supplier>(ordered);
        SelectedSupplier = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private void Add()
    {
        var supplier = new Supplier { Id = Guid.Empty, Name = "" };
        _suppliers.Add(supplier);
        SelectedSupplier = supplier;
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedSupplier is null) return;
        await _service.SaveAsync(SelectedSupplier);
        _statusService.SetStatus("Beszállító mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedSupplier is null) return;
        await _service.DeleteAsync(SelectedSupplier.Id);
        _suppliers.Remove(SelectedSupplier);
        SelectedSupplier = null;
        EnsureValidSelection();
    }
}
