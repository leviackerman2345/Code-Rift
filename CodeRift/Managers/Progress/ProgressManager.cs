using System;
using System.IO;
using System.Text.Json;

namespace CodeRift.Managers
{
    // Level progression tracker with simple JSON persistence.
    public sealed class ProgressManager
    {
        private const int MaxLevel = 5;
        private const int DefaultHighestCompletedLevel = 0; // Unlocks level 1 by default.
        private const string SaveFolderName = "CodeRift";
        private const string SaveFileName = "progress.json";
        private static readonly ProgressManager _instance = new ProgressManager();

        private int _highestCompletedLevel;
        private readonly string _saveFilePath;

        private sealed class ProgressSaveData
        {
            public int HighestCompletedLevel { get; set; }
        }

        private ProgressManager()
        {
            _saveFilePath = BuildSaveFilePath();
            LoadProgress();
        }

        public static ProgressManager Instance => _instance;

        public int UnlockedLevels => Math.Min(MaxLevel, _highestCompletedLevel + 1);

        public void UnlockNextLevel(int completedLevel)
        {
            CompleteLevel(completedLevel);
        }

        public void CompleteLevel(int completedLevel)
        {
            if (completedLevel < 1)
            {
                return;
            }

            int normalizedLevel = Math.Min(completedLevel, MaxLevel);
            if (normalizedLevel <= _highestCompletedLevel)
            {
                return;
            }

            _highestCompletedLevel = normalizedLevel;
            SaveProgress();
        }

        public bool IsLevelUnlocked(int level)
        {
            return level >= 1 && level <= UnlockedLevels;
        }

        private static string BuildSaveFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appDataPath, SaveFolderName, SaveFileName);
        }

        private void LoadProgress()
        {
            _highestCompletedLevel = DefaultHighestCompletedLevel;

            try
            {
                if (!File.Exists(_saveFilePath))
                {
                    return;
                }

                string json = File.ReadAllText(_saveFilePath);
                ProgressSaveData? saveData = JsonSerializer.Deserialize<ProgressSaveData>(json);
                if (saveData == null)
                {
                    return;
                }

                _highestCompletedLevel = ClampHighestCompletedLevel(saveData.HighestCompletedLevel);
            }
            catch
            {
                // Corrupted save files fallback safely to default unlock state.
                _highestCompletedLevel = DefaultHighestCompletedLevel;
            }
        }

        private void SaveProgress()
        {
            try
            {
                string? directoryPath = Path.GetDirectoryName(_saveFilePath);
                if (!string.IsNullOrWhiteSpace(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                ProgressSaveData saveData = new ProgressSaveData
                {
                    HighestCompletedLevel = _highestCompletedLevel
                };

                string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(_saveFilePath, json);
            }
            catch
            {
                // Save failures should not break gameplay flow.
            }
        }

        private static int ClampHighestCompletedLevel(int level)
        {
            if (level < DefaultHighestCompletedLevel)
            {
                return DefaultHighestCompletedLevel;
            }

            if (level > MaxLevel)
            {
                return MaxLevel;
            }

            return level;
        }
    }
}
