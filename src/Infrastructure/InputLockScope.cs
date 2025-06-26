namespace Wrecept.Infrastructure;

public sealed class InputLockScope : IDisposable
{
    public InputLockScope()
    {
        AppContext.InputLocked = true;
    }

    public void Dispose()
    {
        AppContext.InputLocked = false;
    }
}
