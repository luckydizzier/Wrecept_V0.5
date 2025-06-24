namespace Wrecept.Core.Domain;

public record class PaymentMethod
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
}
