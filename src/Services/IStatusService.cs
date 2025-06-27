namespace Wrecept.Services;

public interface IStatusService
{
    Action<string>? StatusMessageSetter { get; set; }
    void SetStatus(string message);
}
