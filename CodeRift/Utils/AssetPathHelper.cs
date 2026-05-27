using System;
using System.IO;
using System.Windows.Forms;

namespace CodeRift.Utils
{
    public static class AssetPathHelper
    {
        public static string ResolveAssetPath(params string[] relativeSegments)
        {
            string relativePath = Path.Combine(relativeSegments);
            string outputPath = Path.Combine(Application.StartupPath, relativePath);
            if (File.Exists(outputPath) || Directory.Exists(outputPath))
            {
                return outputPath;
            }

            return Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", "..", relativePath));
        }
    }
}
