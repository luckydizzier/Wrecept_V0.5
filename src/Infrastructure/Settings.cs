namespace Wrecept.Infrastructure;

public class Settings
{
    public string Theme { get; set; } = "Light";
    public Guid? LastSupplierFilterId { get; set; }
    public double WindowWidth { get; set; } = 800;
    public double WindowHeight { get; set; } = 600;
}
