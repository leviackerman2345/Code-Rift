using System;

namespace CodeRift.Managers
{
    // In-memory progression tracker for level lock/unlock state.
    // Defense note: not persisted to disk yet (resets on app restart).
    public sealed class ProgressManager
    {
        private const int MaxLevel = 5;
        private static readonly ProgressManager _instance = new ProgressManager();

        private int _highestCompletedLevel;

        private ProgressManager()
        {
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

            _highestCompletedLevel = Math.Max(_highestCompletedLevel, Math.Min(completedLevel, MaxLevel));
        }

        public bool IsLevelUnlocked(int level)
        {
            return level >= 1 && level <= UnlockedLevels;
        }
    }
}
