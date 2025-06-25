namespace Wrecept.Core.Domain;

public record class Unit
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
}
