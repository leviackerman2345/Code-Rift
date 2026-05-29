using System;

namespace CodeRift.Core
{
    public class AssetLoadProgress
    {
        public AssetLoadProgress()
        {
            AssetName = string.Empty;
            Message = string.Empty;
        }

        public int LoadedCount { get; set; }

        public int TotalCount { get; set; }

        public string AssetName { get; set; }

        public string Message { get; set; }

        public int Percent { get { return TotalCount <= 0 ? 0 : (int)((double)LoadedCount / TotalCount * 100); } }
    }
}
