using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CodeRift.Managers
{
    // Simple localization service backed by Utils/<lang>.json files.
    public sealed class LanguageManager
    {
        private static readonly LanguageManager _instance = new LanguageManager();
        private readonly Dictionary<string, string> _strings = new Dictionary<string, string>(StringComparer.Ordinal);

        private LanguageManager()
        {
            CurrentLanguage = string.Empty;
        }

        public static LanguageManager Instance { get { return _instance; } }

        public string CurrentLanguage { get; private set; }

        public void Load(string languageCode)
        {
            CurrentLanguage = languageCode;
            _strings.Clear();

            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return;
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utils", string.Format("{0}.json", languageCode));
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (values == null)
                {
                    return;
                }

                foreach (KeyValuePair<string, string> pair in values)
                {
                    _strings[pair.Key] = pair.Value;
                }
            }
            catch (JsonSerializationException)
            {
            }
            catch (IOException)
            {
            }
        }

        public string Get(string key)
        {
            // Missing keys are surfaced intentionally for easier UI text debugging.
            string value;
            if (_strings.TryGetValue(key, out value))
                return value;
            return string.Format("[{0}]", key);
        }

        public void Switch(string languageCode)
        {
            Load(languageCode);
        }
    }
}
