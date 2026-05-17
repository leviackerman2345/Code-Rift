using System;

namespace CodeRift.Managers
{
    public sealed class ProgressManager
    {
        private static readonly ProgressManager _instance = new ProgressManager();
        
        // Tracks the highest level unlocked (1 to 5)
        public int UnlockedLevels { get; private set; } = 1;

        private ProgressManager()
        {
        }

        public static ProgressManager Instance => _instance;

        public void UnlockNextLevel(int completedLevel)
        {
            if (completedLevel >= UnlockedLevels && UnlockedLevels < 5)
            {
                UnlockedLevels = completedLevel + 1;
            }
        }

        public bool IsLevelUnlocked(int level)
        {
            return level <= UnlockedLevels;
        }
    }
}
