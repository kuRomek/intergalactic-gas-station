﻿namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения

        public bool[] OpenLevels;
        public bool IsInfiniteGameUnlocked = false;
        public float InfiniteGameRecord = 0f;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            OpenLevels = new bool[10];
            OpenLevels[0] = true;
            
            for (int i = 1; i < OpenLevels.Length; i++)
                OpenLevels[i] = false;
        }
    }
}