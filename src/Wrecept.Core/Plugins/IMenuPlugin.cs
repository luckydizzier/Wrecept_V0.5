namespace Wrecept.Core.Plugins;

public interface IMenuPlugin
{
    string MenuHeader { get; }
    void Execute();
}
