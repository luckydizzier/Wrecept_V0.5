using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class TaxRateListViewModel : RestorableListViewModel<TaxRate>
{
    private readonly ITaxRateService _service;

    private ObservableCollection<TaxRate> _rates = new();

    protected override IList<TaxRate> Items => _rates;

    public ObservableCollection<TaxRate> Rates
    {
        get => _rates;
        private set => SetProperty(ref _rates, value);
    }

    public TaxRate? SelectedRate
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    public TaxRateListViewModel(ITaxRateService service)
    {
        _service = service;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        var ordered = items.OrderByDescending(t => t.Id);
        Rates = new ObservableCollection<TaxRate>(ordered);
        SelectedRate = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private void Add()
    {
        var rate = new TaxRate { Id = Guid.Empty, Label = string.Empty, Percentage = 0 };
        _rates.Add(rate);
        SelectedRate = rate;
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedRate is null) return;
        await _service.SaveAsync(SelectedRate);
        Infrastructure.AppContext.SetStatus("ÁFA-kulcs mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedRate is null) return;
        await _service.DeleteAsync(SelectedRate.Id);
        _rates.Remove(SelectedRate);
        SelectedRate = null;
        EnsureValidSelection();
    }
}
