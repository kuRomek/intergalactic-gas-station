using YG;

public static class PlayerProgressController
{
    public static void RemoveTutorialOnLevel(int levelNumber)
    {
        YandexGame.savesData.ShownTutorials[levelNumber - 1] = true;
        YandexGame.SaveProgress();
    }

    public static void CompleteLevel(int levelNumber)
    {
        int levelIndex = levelNumber - 1;

        if (levelNumber < 10)
            YandexGame.savesData.OpenLevels[levelIndex + 1] = true;
        else if (levelNumber == 10)
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
        YandexGame.NewLeaderboardScores(nameLB: "Leaderboard", score: (long)recordTime * 1000);
        YandexGame.SaveProgress();
    }
}
