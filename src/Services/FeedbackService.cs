using System;

namespace Wrecept.Services;

public class FeedbackService : IFeedbackService
{
    private readonly Action<int, int> _beep;

    public FeedbackService() : this((f, d) =>
    {
        if (OperatingSystem.IsWindows())
            Console.Beep(f, d);
    }) { }

    public FeedbackService(Action<int, int> beep)
    {
        _beep = beep;
    }

    public void Startup() => Play(300, 600, 1200);
    public void Exit() => Play(1200, 600, 300);
    public void Accept() => Play(600, 1200);
    public void Reject() => Play(1200, 600);
    public void Error() => Play(400, 400);

    private void Play(params int[] freqs)
    {
        foreach (var f in freqs)
        {
            try
            {
                _beep(f, 100);
            }
            catch
            {
                if (OperatingSystem.IsWindows())
                    System.Media.SystemSounds.Beep.Play();
            }
        }
    }
}
