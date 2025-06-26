namespace Wrecept.Infrastructure;

public class Settings
{
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "hu";
    public Guid? LastSupplierFilterId { get; set; }
    public double WindowWidth { get; set; } = 800;
    public double WindowHeight { get; set; } = 600;
    public bool ShowOnboarding { get; set; } = true;
    public int FontScale { get; set; } = 0;
}
