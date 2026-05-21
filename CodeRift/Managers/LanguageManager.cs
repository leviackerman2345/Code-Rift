using System.IO;
using System.Text.Json;

namespace CodeRift.Managers
{
    // Simple localization service backed by Utils/<lang>.json files.
    public sealed class LanguageManager
    {
        private static readonly LanguageManager _instance = new LanguageManager();
        private readonly Dictionary<string, string> _strings = new(StringComparer.Ordinal);

        private LanguageManager()
        {
        }

        public static LanguageManager Instance => _instance;

        public string CurrentLanguage { get; private set; } = string.Empty;

        public void Load(string languageCode)
        {
            CurrentLanguage = languageCode;
            _strings.Clear();

            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return;
            }

            string path = Path.Combine(AppContext.BaseDirectory, "Utils", $"{languageCode}.json");
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                Dictionary<string, string>? values = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (values == null)
                {
                    return;
                }

                foreach (KeyValuePair<string, string> pair in values)
                {
                    _strings[pair.Key] = pair.Value;
                }
            }
            catch (JsonException)
            {
            }
            catch (IOException)
            {
            }
        }

        public string Get(string key)
        {
            // Missing keys are surfaced intentionally for easier UI text debugging.
            return _strings.TryGetValue(key, out string? value) ? value : $"[{key}]";
        }

        public void Switch(string languageCode)
        {
            Load(languageCode);
        }
    }
}
