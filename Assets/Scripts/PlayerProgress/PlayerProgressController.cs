using YG;

namespace PlayerProgress
{
    public static class PlayerProgressController
    {
        private static long _milisecondsInOneSecond = 1000;
        private static int _levelCount = 10;
        private static string _leaderboardName = "Leaderboard";

        public static void RemoveTutorialOnLevel(int levelNumber)
        {
            YandexGame.savesData.ShownTutorials[levelNumber - 1] = true;
            YandexGame.SaveProgress();
        }

        public static void CompleteLevel(int levelNumber)
        {
            int levelIndex = levelNumber - 1;

            if (levelNumber < _levelCount)
                YandexGame.savesData.OpenLevels[levelIndex + 1] = true;
            else if (levelNumber == _levelCount)
                YandexGame.savesData.IsInfiniteGameUnlocked = true;
            else
                throw new System.InvalidOperationException($"Game does not have level #{levelNumber}");

            YandexGame.SaveProgress();
        }

        public static void UpdateInfiniteGameRecord(float recordTime)
        {
            if (YandexGame.savesData.InfiniteGameRecord > recordTime)
                return;

            YandexGame.savesData.InfiniteGameRecord = recordTime;
            YandexGame.NewLeaderboardScores(
                nameLB: _leaderboardName, score: (long)recordTime * _milisecondsInOneSecond);
            YandexGame.SaveProgress();
        }
    }
}
