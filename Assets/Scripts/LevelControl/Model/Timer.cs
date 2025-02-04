using System;
using YG;
using StructureElements;

namespace LevelControl
{
    public class Timer : IUpdatable, IActivatable
    {
        private bool _isExpired = false;
        private float _timeAddingReward = 30f;

        public Timer(float seconds)
        {
            SecondsLeft = seconds;
            SecondsPassed = 0f;
            Resume();
        }

        public event Action Expired;

        public event Action TimeChanged;

        public event Action<float> TimeAdded;

        public float SecondsLeft { get; private set; }

        public float SecondsPassed { get; private set; }

        public bool IsRunning { get; private set; }

        public void Update(float deltaTime)
        {
            if (IsRunning)
            {
                SecondsLeft = MathF.Max(0f, SecondsLeft - deltaTime);
                SecondsPassed += deltaTime;
                TimeChanged?.Invoke();

                if (SecondsLeft == 0)
                {
                    if (_isExpired == false)
                    {
                        Stop();
                        Expired?.Invoke();
                        _isExpired = true;
                    }
                }
            }
        }

        public void Enable()
        {
            YandexGame.RewardVideoEvent += OnRewardedVideoWatched;
        }

        public void Disable()
        {
            YandexGame.RewardVideoEvent += OnRewardedVideoWatched;
        }

        public void Stop()
        {
            IsRunning = false;
            YandexGame.GameplayStop();
        }

        public void Resume()
        {
            IsRunning = true;
            YandexGame.GameplayStart();
        }

        public void AddTime(float seconds)
        {
            SecondsLeft += seconds;
            TimeAdded?.Invoke(seconds);
        }

        public void OnRewardedVideoWatched(int id)
        {
            if (id == 1)
            {
                AddTime(_timeAddingReward);
                Resume();
                _isExpired = false;
            }
        }
    }
}
