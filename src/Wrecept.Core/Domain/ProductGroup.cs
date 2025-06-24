namespace Wrecept.Core.Domain;

public record class ProductGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
