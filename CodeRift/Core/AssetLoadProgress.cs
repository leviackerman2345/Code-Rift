namespace CodeRift.Core
{
    public class AssetLoadProgress
    {
        public int LoadedCount { get; set; }

        public int TotalCount { get; set; }

        public string AssetName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int Percent => TotalCount <= 0 ? 0 : (int)((double)LoadedCount / TotalCount * 100);
    }
}
