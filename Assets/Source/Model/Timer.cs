using System;

public class Timer : IUpdatable
{
    public Timer(float seconds)
    {
        SecondsLeft = seconds;
        SecondsPassed = 0f;
        IsRunning = true;
    }

    public event Action Expired;
    public event Action TimeChanged;

    public void Update(float deltaTime)
    {
        if (IsRunning)
        {
            SecondsLeft = MathF.Max(0f, SecondsLeft - deltaTime);
            SecondsPassed += deltaTime;
            TimeChanged?.Invoke();
            
            if (SecondsLeft == 0)
            {
                Stop();
                Expired?.Invoke();
            }
        }
    }

    public float SecondsLeft { get; private set; }
    public float SecondsPassed { get; private set; }
    public bool IsRunning { get; private set; }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Resume()
    {
        IsRunning = true;
    }

    public void AddTime(float seconds)
    {
        SecondsLeft += seconds;
    }
}
