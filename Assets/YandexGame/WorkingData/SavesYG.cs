namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";

        // Ваши сохранения

        public bool[] OpenLevels;
        public bool IsInfiniteGameUnlocked = false;
        public float InfiniteGameRecord = 0f;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            OpenLevels = new bool[10];
            
            // Допустим, задать значения по умолчанию для отдельных элементов массива
            for (int i = 0; i < OpenLevels.Length; i++)
                OpenLevels[i] = true;

            IsInfiniteGameUnlocked = true;
        }
    }
}
