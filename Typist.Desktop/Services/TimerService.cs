using System.Windows.Threading;

namespace Typist.Desktop.Services;

public class TimerService
{
    private readonly DispatcherTimer _timer;
    private readonly System.Diagnostics.Stopwatch _stopwatch = new();
    private int _targetSeconds;
    private bool _isCountdown;

    public event EventHandler<int>? Tick; // Fires remaining seconds (countdown) or elapsed seconds
    public event EventHandler? TimeUp;

    public int RemainingSeconds => Math.Max(0, _targetSeconds - (int)_stopwatch.Elapsed.TotalSeconds);
    public int ElapsedSeconds => (int)_stopwatch.Elapsed.TotalSeconds;
    public double ElapsedPrecise => _stopwatch.Elapsed.TotalSeconds;
    public bool IsRunning => _stopwatch.IsRunning;

    public TimerService()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _timer.Tick += OnTimerTick;
    }

    public void StartCountdown(int totalSeconds)
    {
        _targetSeconds = totalSeconds;
        _isCountdown = true;
        _stopwatch.Restart();
        _timer.Start();
    }

    public void StartElapsed()
    {
        _isCountdown = false;
        _stopwatch.Restart();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        _stopwatch.Stop();
    }

    public void Reset()
    {
        _timer.Stop();
        _stopwatch.Reset();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (_isCountdown)
        {
            int remaining = RemainingSeconds;
            Tick?.Invoke(this, remaining);

            if (remaining <= 0)
            {
                Stop();
                TimeUp?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            Tick?.Invoke(this, ElapsedSeconds);
        }
    }
}
