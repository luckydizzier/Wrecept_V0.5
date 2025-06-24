namespace Wrecept.Core.Domain;

public record class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProductGroup Group { get; set; } = default!;
    public TaxRate TaxRate { get; set; } = default!;
    public Unit DefaultUnit { get; set; } = default!;
}
