using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using PlayerProgress;
using Ships;
using StructureElements;
using UI;

namespace LevelControl
{
    public class LevelState : IActivatable
    {
        private const string Level4 = nameof(Level4);

        private UIMenu _levelCompleteWindow;
        private UIMenu _loseWindow;
        private UIMenu _pauseWindow;
        private Button _pauseButton;
        private List<Ship> _ships;
        private Station _station;
        private Timer _timer;

        public LevelState(
            UIMenu levelCompleteWindow,
            UIMenu loseWindow,
            UIMenu pauseWindow,
            Button pauseButton,
            Station station,
            List<Ship> ships,
            Timer timer)
        {
            _levelCompleteWindow = levelCompleteWindow;
            _loseWindow = loseWindow;
            _pauseWindow = pauseWindow;
            _pauseButton = pauseButton;
            _station = station;
            _ships = ships;
            _timer = timer;

            ShipCountOnLevel = _ships.Count;

            if (SceneManager.GetActiveScene().name == Level4 &&
                YandexGame.savesData.ShownTutorials[SceneManager.GetSceneByName(Level4).buildIndex - 1] == false)
            {
                Ship shipWithTwoTanks = _ships.Find(ship => ship.Tanks.Count > 1);
                _ships.Remove(shipWithTwoTanks);
                _ships.Insert(0, shipWithTwoTanks);
            }
        }

        public event Action ShipRefueled;

        public int ShipCountOnLevel { get; private set; }

        public int RefueledShipCount { get; private set; } = 0;

        public bool IsGameOver { get; private set; } = false;

        public bool IsPaused => Time.timeScale == 0f;

        protected List<Ship> ShipsQueue => _ships;

        protected Timer Timer => _timer;

        public void Enable()
        {
            _pauseWindow.Hid += UnPause;
            _loseWindow.Hid += UnPause;
            _pauseButton.onClick.AddListener(Pause);
            _timer.Expired += OnTimerExpired;
            _station.PlaceFreed += OnStationPlaceFreed;
            _timer.Enable();
            YandexGame.RewardVideoEvent += OnRewardedVideoWatched;
            Application.focusChanged += OnFocusChanged;
        }

        public void Disable()
        {
            _pauseWindow.Hid -= UnPause;
            _loseWindow.Hid -= UnPause;
            _pauseButton.onClick.RemoveListener(Pause);
            _timer.Expired -= OnTimerExpired;
            _station.PlaceFreed -= OnStationPlaceFreed;
            _timer.Disable();
            YandexGame.RewardVideoEvent -= OnRewardedVideoWatched;
            Application.focusChanged -= OnFocusChanged;
        }

        public void Pause()
        {
            if (IsGameOver)
                return;

            _pauseButton.gameObject.SetActive(false);
            Time.timeScale = 0f;
            _timer.Stop();
            _pauseWindow.Show();
        }

        public void UnPause()
        {
            _pauseButton.gameObject.SetActive(true);
            Time.timeScale = 1f;
            _timer.Resume();
        }

        protected virtual void OnStationPlaceFreed(Ship ship)
        {
            RefueledShipCount++;
            LetShipOnStation();
            ShipRefueled?.Invoke();
        }

        protected void LetShipOnStation()
        {
            if (_ships.Count > 0)
            {
                _station.Arrive(_ships[0]);
                _ships.RemoveAt(0);
            }
            else if (RefueledShipCount == ShipCountOnLevel)
            {
                IsGameOver = true;
                _timer.Stop();
                _levelCompleteWindow.Show();
                _pauseButton.gameObject.SetActive(false);
                PlayerProgressController.CompleteLevel(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnFocusChanged(bool isVisible)
        {
            if (isVisible == false)
                Pause();
        }

        private void OnTimerExpired()
        {
            Time.timeScale = 0f;
            IsGameOver = true;
            _loseWindow.Show();
            _pauseButton.gameObject.SetActive(false);

            if (this is InfiniteLevelState)
                PlayerProgressController.UpdateInfiniteGameRecord(_timer.SecondsPassed);
        }

        private void OnRewardedVideoWatched(int id)
        {
            if (id == 1)
            {
                Time.timeScale = 1f;
                IsGameOver = false;
                _loseWindow.Hide();
                _pauseButton.gameObject.SetActive(true);
            }
        }
    }
}
