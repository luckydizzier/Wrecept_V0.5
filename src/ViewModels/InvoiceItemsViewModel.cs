using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public partial class InvoiceItemsViewModel : ObservableObject
{
    public Invoice Invoice { get; }
    public ObservableCollection<InvoiceItemRowViewModel> Rows { get; }
    public InvoiceItemRowViewModel Entry { get; }

    public IRelayCommand AddItemCommand { get; }

    public InvoiceItemsViewModel(Invoice invoice)
    {
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

    private void AddItem()
    {
        if (!Entry.Validate())
            return;

        var model = Entry.ToModel();
        Invoice.Items.Add(model);
        Rows.Add(new InvoiceItemRowViewModel(model));
        Entry.Clear();
        OnPropertyChanged(nameof(Rows));
    }
}
