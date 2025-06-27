using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
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
    private readonly IPriceHistoryService _historyService;
    private readonly IFeedbackService _feedbackService;
    public LookupBoxViewModel<Product> ProductLookup { get; }
    public LookupBoxViewModel<Unit> UnitLookup { get; }
    public LookupBoxViewModel<TaxRate> TaxRateLookup { get; }

    public IRelayCommand<int> StartEditCommand { get; }
    public IAsyncRelayCommand<int> ConfirmEntryCommand { get; }
    public IRelayCommand CancelEntryCommand { get; }


    public InvoiceItemsViewModel(
        Invoice invoice,
        IProductService productService,
        IProductGroupService groupService,
        IUnitService unitService,
        ITaxRateService taxService,
        IPriceHistoryService historyService,
        IFeedbackService feedbackService)
    {
        _productService = productService;
        _groupService = groupService;
        _unitService = unitService;
        _taxService = taxService;
        _historyService = historyService;
        _feedbackService = feedbackService;

        Invoice = invoice;
        Rows = new ObservableCollection<InvoiceItemRowViewModel>();
        Entry = new InvoiceItemRowViewModel { IsPlaceholder = true };
        Rows.Add(Entry);
        foreach (var item in invoice.Items)
        {
            Rows.Add(new InvoiceItemRowViewModel(item));
        }
        AddItemCommand = new RelayCommand(AddItem);
        StartEditCommand = new RelayCommand<int>(StartEdit);
        ConfirmEntryCommand = new AsyncRelayCommand<int>(ConfirmEntryAsync);
        CancelEntryCommand = new RelayCommand(CancelEntry);
        ProductLookup = new LookupBoxViewModel<Product>(SearchProductsAsync, p => p.Name, OnProductSelected, () => { });
        UnitLookup = new LookupBoxViewModel<Unit>(SearchUnitsAsync, u => u.Name, OnUnitSelected, () => { });
        TaxRateLookup = new LookupBoxViewModel<TaxRate>(SearchTaxRatesAsync, t => t.Label, OnTaxRateSelected, () => { });
    }

    public bool LastAddSuccess { get; private set; }

    private void AddItem()
    {
        if (!Entry.Validate())
        {
            LastAddSuccess = false;
            _feedbackService.Error();
            return;
        }

        var model = Entry.ToModel();
        Invoice.Items.Add(model);
        Rows.Add(new InvoiceItemRowViewModel(model));
        Entry.Clear();
        LastAddSuccess = true;
        _feedbackService.Accept();
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

    private async Task<List<Product>> SearchProductsAsync(string term)
    {
        var all = await _productService.GetAllAsync();
        return all.Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private async Task<List<Unit>> SearchUnitsAsync(string term)
    {
        var all = await _unitService.GetAllAsync();
        return all.Where(u => u.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private async Task<List<TaxRate>> SearchTaxRatesAsync(string term)
    {
        var all = await _taxService.GetAllAsync();
        return all.Where(t => t.Label.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private void OnProductSelected(Product product)
    {
        Entry.ProductName = product.Name;
        var price = _historyService.GetLatestPrice(product.Name);
        if (price.HasValue)
            Entry.UnitPriceNet = price.Value;
    }

    private void OnUnitSelected(Unit unit)
    {
        Entry.UnitName = unit.Name;
    }

    private void OnTaxRateSelected(TaxRate rate)
    {
        Entry.VatRatePercent = rate.Percentage;
    }

    private void StartEdit(int column)
    {
        if (column == 0)
            OpenProductLookup();
        else if (column == 2)
            OpenUnitLookup();
        else if (column == 4)
            OpenTaxRateLookup();
    }

    private async Task ConfirmEntryAsync(int column)
    {
        if (column == 0)
        {
            await TryOpenProductCreatorAsync();
            return;
        }

        if (column == 4)
        {
            if (AddItemCommand.CanExecute(null))
                AddItemCommand.Execute(null);
        }
    }

    private void CancelEntry()
    {
        Entry.Clear();
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

    public void OpenProductLookup()
    {
        ProductLookup.Open();
    }

    public void OpenUnitLookup()
    {
        UnitLookup.Open();
    }

    public void OpenTaxRateLookup()
    {
        TaxRateLookup.Open();
    }
}
