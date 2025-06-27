namespace Wrecept.Core.Domain;

public record class Invoice
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public Supplier Supplier { get; set; } = default!;
    public CalculationMode CalculationMode { get; set; }
    public List<InvoiceItem> Items { get; set; } = new();
    public PaymentMethod PaymentMethod { get; set; } = default!;
    public string Notes { get; set; } = string.Empty;
}
