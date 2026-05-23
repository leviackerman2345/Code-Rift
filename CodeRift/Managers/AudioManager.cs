using CodeRift.Utils;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
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
            { Constants.SFX_CLICK, @"Assets\Audio\sfx\mouse click.mp3" },
            { Constants.SFX_HOVER, @"Assets\Audio\sfx\hoverbtnsfx.mp3" },
            { Constants.SFX_HIT, @"Assets\Audio\sfx\hit.wav" },
            { Constants.SFX_DEATH, @"Assets\Audio\sfx\death.wav" }
        };
        private readonly Dictionary<string, string> _music = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.MUSIC_MENU, @"Assets\Audio\music\MainMenuBGMusic.mp3" },
            { Constants.MUSIC_PROLOGUE, @"Assets\Audio\music\PrologueBGMusic.mp3" },
            { Constants.MUSIC_LEVELS, @"Assets\Audio\music\LevelsBGMusic.mp3" },
            { Constants.MUSIC_EPILOGUE, @"Assets\Audio\music\EpilogueBGMusic.mp3" }
        };
        private readonly Dictionary<string, string> _cgAudio = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.SFX_CG_CLICK, @"Assets\Audio\sfx\cg_click.wav" },
            { Constants.SFX_CG_END, @"Assets\Audio\sfx\cg_end.wav" },
            { Constants.SFX_CG_ENTER, @"Assets\Audio\sfx\cg_enter.wav" }
        };

        private string? _currentMusicKey;
        private int _globalVolume = 800; // Default 80% (0-1000 scale for MCI)
        private bool _sfxEnabled = true;

        public int VolumePercent => _globalVolume / 10;
        public bool IsSFXEnabled 
        { 
            get => _sfxEnabled; 
            set => _sfxEnabled = value; 
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder? strReturn, int iReturnLength, IntPtr hwndCallback);

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

            if (path.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                _sounds[key] = File.ReadAllBytes(resolvedPath);
            }
        }

        public void PlaySFX(string key)
        {
            if (!_sfxEnabled) return;

            // Case 1: Preloaded WAV SFX
            if (_sounds.TryGetValue(key, out byte[]? bytes))
            {
                Task.Run(() =>
                {
                    using MemoryStream stream = new MemoryStream(bytes, writable: false);
                    using SoundPlayer player = new SoundPlayer(stream);
                    player.PlaySync();
                });
                return;
            }

            // Case 2: File-based SFX (e.g. MP3)
            string? path = null;
            if (!_sfx.TryGetValue(key, out path))
            {
                if (!_cgAudio.TryGetValue(key, out path)) return;
            }

            string resolvedPath = ResolvePath(path);
            if (!File.Exists(resolvedPath)) return;

            Task.Run(() => 
            {
                string alias = $"SFX_{key}_{Guid.NewGuid():N}";
                mciSendString($"open \"{resolvedPath}\" type mpegvideo alias {alias}", null, 0, IntPtr.Zero);
                mciSendString($"setaudio {alias} volume to {_globalVolume}", null, 0, IntPtr.Zero);
                mciSendString($"play {alias} wait", null, 0, IntPtr.Zero);
                mciSendString($"close {alias}", null, 0, IntPtr.Zero);
            });
        }

        public void PlayMusic(string key, bool loop = true)
        {
            if (_currentMusicKey == key) return;

            StopMusic();

            string? path = null;
            if (!_music.TryGetValue(key, out path)) return;

            string resolvedPath = ResolvePath(path);
            if (!File.Exists(resolvedPath)) return;

            _currentMusicKey = key;
            
            string command = $"open \"{resolvedPath}\" type mpegvideo alias MyMusic";
            mciSendString(command, null, 0, IntPtr.Zero);
            
            // Apply current global volume to the new track
            mciSendString($"setaudio MyMusic volume to {_globalVolume}", null, 0, IntPtr.Zero);

            command = "play MyMusic" + (loop ? " repeat" : "");
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void SetVolume(int volumePercent)
        {
            // MCI volume is 0 to 1000
            _globalVolume = Math.Clamp(volumePercent * 10, 0, 1000);
            
            if (_currentMusicKey != null)
            {
                mciSendString($"setaudio MyMusic volume to {_globalVolume}", null, 0, IntPtr.Zero);
            }
        }

        public void StopMusic()
        {
            mciSendString("stop MyMusic", null, 0, IntPtr.Zero);
            mciSendString("close MyMusic", null, 0, IntPtr.Zero);
            _currentMusicKey = null;
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
