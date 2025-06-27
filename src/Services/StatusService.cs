namespace Wrecept.Services;

public class StatusService : IStatusService
{
    public Action<string>? StatusMessageSetter { get; set; }

    public void SetStatus(string message) => StatusMessageSetter?.Invoke(message);
}
