using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class UnitListViewModel : RestorableListViewModel<Unit>
{
    private readonly IUnitService _service;
    private readonly IStatusService _statusService;

    private ObservableCollection<Unit> _units = new();

    protected override IList<Unit> Items => _units;

    public ObservableCollection<Unit> Units
    {
        get => _units;
        private set => SetProperty(ref _units, value);
    }

    public Unit? SelectedUnit
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    public UnitListViewModel(IUnitService service, IStatusService statusService)
    {
        _service = service;
        _statusService = statusService;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        var ordered = items.OrderByDescending(u => u.Id);
        Units = new ObservableCollection<Unit>(ordered);
        SelectedUnit = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private void Add()
    {
        var unit = new Unit { Id = Guid.Empty, Name = string.Empty, Symbol = string.Empty };
        _units.Add(unit);
        SelectedUnit = unit;
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedUnit is null) return;
        await _service.SaveAsync(SelectedUnit);
        _statusService.SetStatus("Mértékegység mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedUnit is null) return;
        await _service.DeleteAsync(SelectedUnit.Id);
        _units.Remove(SelectedUnit);
        SelectedUnit = null;
        EnsureValidSelection();
    }
}
