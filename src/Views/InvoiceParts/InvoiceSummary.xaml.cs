using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;
using System.Windows.Controls;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceSummary : UserControl
{
    public InvoiceSummary()
    {
        InitializeComponent();
        Loaded += (_, _) => App.Services.GetRequiredService<IFocusService>().SetInitialFocus(this);
    }
}
