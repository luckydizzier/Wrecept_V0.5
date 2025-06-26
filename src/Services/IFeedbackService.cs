namespace Wrecept.Services;

public interface IFeedbackService
{
    void Startup();
    void Exit();
    void Accept();
    void Reject();
    void Error();
}
