namespace Wrecept.Services;

using System.Windows.Controls;

public interface IFocusService
{
    void SetInitialFocus(UserControl view);
    void Focus(Control control);
}
