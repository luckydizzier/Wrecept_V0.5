using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class ProductListViewModel : RestorableListViewModel<Product>
{
    private readonly IProductService _service;
    private readonly IProductGroupService _groupService;
    private readonly ITaxRateService _taxService;
    private readonly IUnitService _unitService;
    private readonly IStatusService _statusService;

    private ObservableCollection<Product> _products = new();


    protected override IList<Product> Items => _products;

    public ObservableCollection<Product> Products
    {
        get => _products;
        private set => SetProperty(ref _products, value);
    }

    public Product? SelectedProduct
    {
        get => SelectedItem;
        set => SelectedItem = value;
    }

    public ProductListViewModel(
        IProductService service,
        IProductGroupService groupService,
        ITaxRateService taxService,
        IUnitService unitService,
        IStatusService statusService)
    {
        _service = service;
        _groupService = groupService;
        _taxService = taxService;
        _unitService = unitService;
        _statusService = statusService;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        var ordered = items.OrderByDescending(p => p.Id);
        Products = new ObservableCollection<Product>(ordered);
        SelectedProduct = GetDefaultSelection();
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var groups = await _groupService.GetAllAsync();
        var taxes = await _taxService.GetAllAsync();
        var units = await _unitService.GetAllAsync();

        if (!groups.Any() || !taxes.Any() || !units.Any())
        {
            _statusService.SetStatus("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.");
            return;
        }

        var product = new Product
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Group = groups.First(),
            TaxRate = taxes.First(),
            DefaultUnit = units.First()
        };

        _products.Add(product);
        SelectedProduct = product;
        EnsureValidSelection();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedProduct is null) return;
        await _service.SaveAsync(SelectedProduct);
        _statusService.SetStatus("Termék mentve");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedProduct is null) return;
        await _service.DeleteAsync(SelectedProduct.Id);
        _products.Remove(SelectedProduct);
        SelectedProduct = null;
        EnsureValidSelection();
    }
}
