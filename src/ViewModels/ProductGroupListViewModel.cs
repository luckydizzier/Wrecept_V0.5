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

public partial class ProductGroupListViewModel : RestorableListViewModel<ProductGroup>
{
    private readonly IProductGroupService _service;
    private readonly IStatusService _statusService;
    private readonly INavigationService _navigation;

    private ObservableCollection<ProductGroup> _groups = new();

    protected override IList<ProductGroup> Items => _groups;

    public ObservableCollection<ProductGroup> Groups
    {
        get => _groups;
        private set => SetProperty(ref _groups, value);
    }

    public ProductGroup? SelectedGroup
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    public ProductGroupListViewModel(IProductGroupService service, IStatusService statusService)
        : this(service, statusService, App.Services.GetRequiredService<INavigationService>())
    {
    }

    public ProductGroupListViewModel(IProductGroupService service, IStatusService statusService, INavigationService navigation)
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
        var ordered = items.OrderByDescending(g => g.Id);
        Groups = new ObservableCollection<ProductGroup>(ordered);
        SelectedGroup = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private void Add()
    {
        var group = new ProductGroup { Id = Guid.Empty, Name = string.Empty };
        _groups.Add(group);
        SelectedGroup = group;
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedGroup is null) return;
        await _service.SaveAsync(SelectedGroup);
        _statusService.SetStatus("Termékcsoport mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedGroup is null) return;
        await _service.DeleteAsync(SelectedGroup.Id);
        _groups.Remove(SelectedGroup);
        SelectedGroup = null;
        EnsureValidSelection();
    }
}
