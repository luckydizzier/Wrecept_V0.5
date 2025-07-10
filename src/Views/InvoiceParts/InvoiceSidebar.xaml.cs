using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;
using System.Windows.Controls;


namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceSidebar : UserControl
{
    public InvoiceSidebar()
    {
        InitializeComponent();
        Loaded += (_, _) => App.Services.GetRequiredService<IFocusService>().SetInitialFocus(this);
    }

}
