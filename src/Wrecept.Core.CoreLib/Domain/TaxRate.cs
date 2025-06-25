namespace Wrecept.Core.Domain;

public record class TaxRate
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
}
