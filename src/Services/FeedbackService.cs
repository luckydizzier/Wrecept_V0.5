using System;

namespace Wrecept.Services;

public class FeedbackService : IFeedbackService
{
    private readonly Action<int, int> _beep;

    public FeedbackService() : this(Console.Beep) { }

    public FeedbackService(Action<int, int> beep)
    {
        _beep = beep;
    }

    public void Startup() => Play(600, 800, 1000);
    public void Exit() => Play(1000, 800, 600);
    public void Accept() => Play(800, 1000);
    public void Reject() => Play(1000, 800);
    public void Error() => Play(500, 500);

    private void Play(params int[] freqs)
    {
        foreach (var f in freqs)
        {
            try
            {
                _beep(f, 150);
            }
            catch
            {
                System.Media.SystemSounds.Beep.Play();
            }
        }
    }
}
