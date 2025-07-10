using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.Lookup;

public partial class LookupDialog : UserControl
{
    public LookupDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => App.Services.GetRequiredService<IFocusService>().SetInitialFocus(this);
    }

}
