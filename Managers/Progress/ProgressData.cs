namespace CodeRift.Managers.Progress
{
    /// <summary>
    /// Serializable container for player progress saved to progress.json.
    /// </summary>
    public sealed class ProgressData
    {
        public int HighestLevelCompleted { get; set; }
    }
}
