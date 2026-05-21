using CodeRift.Utils;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace CodeRift.Managers
{
    public sealed class AudioManager
    {
        private static readonly AudioManager _instance = new AudioManager();
        private readonly Dictionary<string, byte[]> _sounds = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _sfx = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.SFX_JUMP, @"Assets\Audio\sfx\jump.wav" },
            { Constants.SFX_CLICK, @"Assets\Audio\sfx\click.wav" },
            { Constants.SFX_HIT, @"Assets\Audio\sfx\hit.wav" },
            { Constants.SFX_DEATH, @"Assets\Audio\sfx\death.wav" }
        };
        private readonly Dictionary<string, string> _music = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.MUSIC_MENU, @"Assets\Audio\music\menu.wav" },
            { Constants.MUSIC_LEVEL1, @"Assets\Audio\music\level1.wav" },
            { Constants.MUSIC_BOSS, @"Assets\Audio\music\boss.wav" },
            { Constants.MUSIC_CG_EVENT, @"Assets\Audio\music\cg_event.wav" }
        };
        private readonly Dictionary<string, string> _cgAudio = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.SFX_CG_CLICK, @"Assets\Audio\sfx\cg_click.wav" },
            { Constants.SFX_CG_END, @"Assets\Audio\sfx\cg_end.wav" },
            { Constants.SFX_CG_ENTER, @"Assets\Audio\sfx\cg_enter.wav" }
        };
        private MemoryStream? _musicStream;
        private SoundPlayer? _musicPlayer;

        private AudioManager()
        {
        }

        public static AudioManager Instance => _instance;

        public IReadOnlyDictionary<string, string> SFX => _sfx;

        public IReadOnlyDictionary<string, string> Music => _music;

        public IReadOnlyDictionary<string, string> CGAudio => _cgAudio;

        public IEnumerable<KeyValuePair<string, string>> AllAudio => SFX
            .Concat(Music)
            .Concat(CGAudio);

        public void LoadSound(string key, string path)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            string resolvedPath = ResolvePath(path);
            if (!File.Exists(resolvedPath))
            {
                return;
            }

            _sounds[key] = File.ReadAllBytes(resolvedPath);
        }

        public void PlaySFX(string key)
        {
            if (!_sounds.TryGetValue(key, out byte[]? bytes))
            {
                return;
            }

            Task.Run(() =>
            {
                using MemoryStream stream = new MemoryStream(bytes, writable: false);
                using SoundPlayer player = new SoundPlayer(stream);
                player.PlaySync();
            });
        }

        public void PlayMusic(string key, bool loop = true)
        {
            if (!_sounds.TryGetValue(key, out byte[]? bytes))
            {
                return;
            }

            StopMusic();

            _musicStream = new MemoryStream(bytes, writable: false);
            _musicPlayer = new SoundPlayer(_musicStream);

            if (loop)
            {
                _musicPlayer.PlayLooping();
            }
            else
            {
                _musicPlayer.Play();
            }
        }

        public void StopMusic()
        {
            _musicPlayer?.Stop();
            _musicPlayer?.Dispose();
            _musicPlayer = null;

            _musicStream?.Dispose();
            _musicStream = null;
        }

        public void Unload()
        {
            StopMusic();
            _sounds.Clear();
        }

        private static string ResolvePath(string path)
        {
            string normalizedPath = path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            return Path.IsPathRooted(normalizedPath) ? normalizedPath : Path.Combine(AppContext.BaseDirectory, normalizedPath);
        }
    }
}
