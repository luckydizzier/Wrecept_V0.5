using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;

namespace Wrecept.ViewModels;

public partial class InvoiceEditorViewModel : ObservableObject
{
    private readonly Invoice _original;
    [ObservableProperty]
    private Invoice _invoice;

    [ObservableProperty]
    private bool _isEditMode;

    public bool IsReadOnly => !IsEditMode;

    public InvoiceEditorViewModel(Invoice invoice, bool isEditMode)
    {
        _original = invoice;
        _invoice = new Invoice
        {
            Id = invoice.Id,
            SerialNumber = invoice.SerialNumber,
            IssueDate = invoice.IssueDate
        };
        IsEditMode = isEditMode;
    }

    public void CancelEdit()
    {
        Invoice = new Invoice
        {
            Id = _original.Id,
            SerialNumber = _original.SerialNumber,
            IssueDate = _original.IssueDate
        };
    }
}
