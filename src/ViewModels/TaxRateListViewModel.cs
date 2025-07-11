using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class TaxRateListViewModel : RestorableListViewModel<TaxRate>
{
    private readonly ITaxRateService _service;
    private readonly IStatusService _statusService;
    private readonly INavigationService _navigation;

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

    public TaxRateListViewModel(ITaxRateService service, IStatusService statusService)
        : this(service, statusService, App.Services.GetRequiredService<INavigationService>())
    {
    }

    public TaxRateListViewModel(ITaxRateService service, IStatusService statusService, INavigationService navigation)
    {
        _service = service;
        _statusService = statusService;
        _navigation = navigation;
        _ = LoadAsync();
        CloseCommand = new UserRelayCommand(() => { _navigation.CloseCurrentView(); return Task.CompletedTask; }, new KeyGesture(Key.Escape));
    }

    public IUserCommand CloseCommand { get; }

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
        _statusService.SetStatus("ÁFA-kulcs mentve");
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
