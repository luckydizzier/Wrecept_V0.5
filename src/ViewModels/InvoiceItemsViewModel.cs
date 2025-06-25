using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public class InvoiceItemsViewModel : ObservableObject
{
    public Invoice Invoice { get; }

    public InvoiceItemsViewModel(Invoice invoice)
    {
        Invoice = invoice;
    }
}
