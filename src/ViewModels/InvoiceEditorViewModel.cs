using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public partial class InvoiceEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private Invoice _invoice;

    [ObservableProperty]
    private bool _isEditMode;

    public bool IsReadOnly => !IsEditMode;

    public InvoiceEditorViewModel(Invoice invoice, bool isEditMode)
    {
        _invoice = invoice;
        IsEditMode = isEditMode;
    }
}
