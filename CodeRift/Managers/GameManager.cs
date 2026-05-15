using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CodeRift.Models;

namespace CodeRift.Managers
{
    public class GameManager
    {
        private static GameManager? _instance;
        public static GameManager Instance => _instance ??= new GameManager();

        public Player CurrentPlayer { get; private set; }
        public Enemy? CurrentEnemy { get; set; }
        public int CurrentLevel { get; set; }
        public List<int> UnlockedLevels { get; private set; }

        private readonly string _savePath;

        private GameManager()
        {
            _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CodeRift", "progress.json");
            CurrentPlayer = new Player { Name = "Elias", MaxHP = 100, CurrentHP = 100, Level = 1 };
            UnlockedLevels = new List<int> { 1 };
            LoadProgress();
        }

        public bool IsLevelUnlocked(int level) => UnlockedLevels.Contains(level);

        public void UnlockLevel(int level)
        {
            if (!UnlockedLevels.Contains(level))
            {
                UnlockedLevels.Add(level);
                SaveProgress();
            }
        }

        public void ResetPlayerHP()
        {
            CurrentPlayer.CurrentHP = CurrentPlayer.MaxHP;
        }

        public void SaveProgress()
        {
            try
            {
                var directory = Path.GetDirectoryName(_savePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) 
                    Directory.CreateDirectory(directory);

                var data = new { unlockedLevels = UnlockedLevels };
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(_savePath, json);
            }
            catch { /* Ignore for now */ }
        }

        public void LoadProgress()
        {
            try
            {
                if (File.Exists(_savePath))
                {
                    string json = File.ReadAllText(_savePath);
                    var data = JsonSerializer.Deserialize<ProgressData>(json);
                    if (data?.unlockedLevels != null)
                    {
                        UnlockedLevels = data.unlockedLevels;
                    }
                }
            }
            catch { /* Ignore for now */ }
        }

        public void ResetProgress()
        {
            UnlockedLevels = new List<int> { 1 };
            CurrentLevel = 1;
            CurrentPlayer = new Player { Name = "Elias", MaxHP = 100, CurrentHP = 100, Level = 1 };
            SaveProgress();
        }

        private class ProgressData
        {
            public List<int>? unlockedLevels { get; set; }
        }
    }
}
