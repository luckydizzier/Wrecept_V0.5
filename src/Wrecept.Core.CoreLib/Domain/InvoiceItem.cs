namespace Wrecept.Core.Domain;

public record class InvoiceItem
{
    public Guid Id { get; set; }
    public Product Product { get; set; } = default!;
    public decimal Quantity { get; set; }
    public Unit Unit { get; set; } = default!;
    public decimal UnitPriceNet { get; set; }
    public decimal VatRatePercent { get; set; }
}
