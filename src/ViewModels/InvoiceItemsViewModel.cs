using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Views.Lookup;
using Wrecept.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wrecept.ViewModels;

public partial class InvoiceItemsViewModel : ObservableObject
{
    public Invoice Invoice { get; }
    public ObservableCollection<InvoiceItemRowViewModel> Rows { get; }
    public InvoiceItemRowViewModel Entry { get; }
    public InlineProductCreatorViewModel? ProductCreator { get; private set; }

    public IRelayCommand AddItemCommand { get; }

    private readonly IProductService _productService;
    private readonly IProductGroupService _groupService;
    private readonly IUnitService _unitService;
    private readonly ITaxRateService _taxService;
    private readonly ILookupDialogPresenter _lookupPresenter;

    public InvoiceItemsViewModel(Invoice invoice)
        : this(invoice,
            Infrastructure.AppContext.ProductService,
            Infrastructure.AppContext.ProductGroupService,
            Infrastructure.AppContext.UnitService,
            Infrastructure.AppContext.TaxRateService,
            Infrastructure.AppContext.LookupPresenter)
    {
    }

    public InvoiceItemsViewModel(
        Invoice invoice,
        IProductService productService,
        IProductGroupService groupService,
        IUnitService unitService,
        ITaxRateService taxService,
        ILookupDialogPresenter lookupPresenter)
    {
        _productService = productService;
        _groupService = groupService;
        _unitService = unitService;
        _taxService = taxService;
        _lookupPresenter = lookupPresenter;

        Invoice = invoice;
        Rows = new ObservableCollection<InvoiceItemRowViewModel>();
        Entry = new InvoiceItemRowViewModel { IsPlaceholder = true };
        Rows.Add(Entry);
        foreach (var item in invoice.Items)
        {
            Rows.Add(new InvoiceItemRowViewModel(item));
        }
        AddItemCommand = new RelayCommand(AddItem);
    }

    public bool LastAddSuccess { get; private set; }

    private void AddItem()
    {
        if (!Entry.Validate())
        {
            LastAddSuccess = false;
            Infrastructure.AppContext.FeedbackService.Error();
            return;
        }

        var model = Entry.ToModel();
        Invoice.Items.Add(model);
        Rows.Add(new InvoiceItemRowViewModel(model));
        Entry.Clear();
        LastAddSuccess = true;
        Infrastructure.AppContext.FeedbackService.Accept();
        OnPropertyChanged(nameof(Rows));
    }

    public async Task<bool> TryOpenProductCreatorAsync()
    {
        var name = Entry.ProductName.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var existingList = await _productService.GetAllAsync();
        var existing = existingList
            .Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (existing)
            return false;

        ProductCreator = new InlineProductCreatorViewModel(
            _productService,
            _groupService,
            _unitService,
            _taxService,
            name);
        ProductCreator.Saved += OnProductSaved;
        ProductCreator.Canceled += OnProductCanceled;
        OnPropertyChanged(nameof(ProductCreator));
        return true;
    }

    private void OnProductSaved(Product product)
    {
        Entry.ProductName = product.Name;
        ProductCreator = null;
        OnPropertyChanged(nameof(ProductCreator));
    }

    private void OnProductCanceled()
    {
        ProductCreator = null;
        OnPropertyChanged(nameof(ProductCreator));
    }

    public async Task<bool> OpenProductLookupAsync()
    {
        async Task<List<Product>> Search(string term)
        {
            var all = await _productService.GetAllAsync();
            return all.Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var vm = new LookupDialogViewModel<Product>(Search, p => p.Name);
        var result = _lookupPresenter.ShowDialog(vm);
        if (result == true && vm.SelectedItem != null)
        {
            Entry.ProductName = vm.SelectedItem.Value.Name;
            var price = Infrastructure.AppContext.PriceHistoryService.GetLatestPrice(vm.SelectedItem.Value.Name);
            if (price.HasValue)
                Entry.UnitPriceNet = price.Value;
            return true;
        }
        return false;
    }

    public async Task<bool> OpenUnitLookupAsync()
    {
        async Task<List<Unit>> Search(string term)
        {
            var all = await _unitService.GetAllAsync();
            return all.Where(u => u.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var vm = new LookupDialogViewModel<Unit>(Search, u => u.Name);
        var result = _lookupPresenter.ShowDialog(vm);
        if (result == true && vm.SelectedItem != null)
        {
            Entry.UnitName = vm.SelectedItem.Value.Name;
            return true;
        }
        return false;
    }

    public async Task<bool> OpenTaxRateLookupAsync()
    {
        async Task<List<TaxRate>> Search(string term)
        {
            var all = await _taxService.GetAllAsync();
            return all.Where(t => t.Label.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var vm = new LookupDialogViewModel<TaxRate>(Search, t => t.Label);
        var result = _lookupPresenter.ShowDialog(vm);
        if (result == true && vm.SelectedItem != null)
        {
            Entry.VatRatePercent = vm.SelectedItem.Value.Percentage;
            return true;
        }
        return false;
    }
}
