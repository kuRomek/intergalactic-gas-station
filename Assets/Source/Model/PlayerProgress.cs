using System.Linq;
using YG;

public class PlayerProgress
{
    public void CompleteLevel(int levelNumber)
    {
        int levelIndex = levelNumber - 1;

        if (levelNumber < 10)
            YandexGame.savesData.OpenLevels[levelIndex + 1] = true;
        else if (levelNumber > 10)
            throw new System.InvalidOperationException($"Game does not have level #{levelNumber}");

        if (YandexGame.savesData.OpenLevels.All(isCompleted => isCompleted == true))
            YandexGame.savesData.IsInfiniteGameUnlocked = true;

        YandexGame.SaveProgress();
    }

    public void UpdateInfiniteGameRecord(float recordTime)
    {
        YandexGame.savesData.InfiniteGameRecord = recordTime;
        YandexGame.NewLeaderboardScores(nameLB: "Leaderboard", score: (long)recordTime * 1000);
        YandexGame.SaveProgress();
    }
}
