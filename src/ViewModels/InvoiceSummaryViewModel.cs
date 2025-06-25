using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Wrecept.ViewModels;

public class InvoiceSummaryViewModel : ObservableObject
{
    public ObservableCollection<InvoiceEditorViewModel.VatSummary> VatSummaries { get; }
    public InvoiceEditorViewModel.GrandTotal GrandTotals { get; private set; }

    public InvoiceSummaryViewModel(ObservableCollection<InvoiceEditorViewModel.VatSummary> vatSummaries,
                                   InvoiceEditorViewModel.GrandTotal grandTotal)
    {
        VatSummaries = vatSummaries;
        GrandTotals = grandTotal;
    }
}
