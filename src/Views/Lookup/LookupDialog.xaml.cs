using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.Lookup;

public partial class LookupDialog : UserControl
{
    public LookupDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => SearchBox.Focus();
    }

}
