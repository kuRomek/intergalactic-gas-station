using System;

public class Timer : IUpdatable
{
    private float _seconds;
    private bool _running;

    public Timer(float seconds)
    {
        _seconds = seconds;
        _running = true;
    }

    public event Action Expired;
    public event Action TimeChanged;

    public float Seconds => _seconds;

    public void Update(float deltaTime)
    {
        if (_running)
        {
            _seconds -= deltaTime;
            TimeChanged?.Invoke();
            
            if (_seconds <= 0)
            {
                Stop();
                Expired?.Invoke();
            }
        }
    }

    public void Stop()
    {
        _running = false;
    }
}
