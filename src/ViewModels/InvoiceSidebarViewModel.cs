using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using System.Linq;

namespace Wrecept.ViewModels;

public partial class InvoiceSidebarViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Invoice> _invoices;

    [ObservableProperty]
    private Invoice? _selectedInvoice;

    public InvoiceSidebarViewModel(ObservableCollection<Invoice> invoices)
    {
        _invoices = invoices;
        _selectedInvoice = invoices.FirstOrDefault();
    }
}
