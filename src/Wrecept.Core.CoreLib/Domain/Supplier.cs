namespace Wrecept.Core.Domain;

public record class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
}
