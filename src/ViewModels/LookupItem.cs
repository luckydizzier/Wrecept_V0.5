namespace Wrecept.ViewModels;

public class LookupItem<T>(T value, string display)
{
    public T Value { get; } = value;
    public string Display { get; } = display;
}
