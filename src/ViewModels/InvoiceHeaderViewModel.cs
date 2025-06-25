using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public class InvoiceHeaderViewModel : ObservableObject
{
    public Invoice Invoice { get; }
    public IEnumerable<string> PaymentMethods { get; }
    public IEnumerable<string> CalculationModes { get; }

    public InvoiceHeaderViewModel(Invoice invoice,
        IEnumerable<string> paymentMethods,
        IEnumerable<string> calculationModes)
    {
        Invoice = invoice;
        PaymentMethods = paymentMethods;
        CalculationModes = calculationModes;
    }
}
