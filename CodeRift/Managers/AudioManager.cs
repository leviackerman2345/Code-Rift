using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CodeRift.Utils;

namespace CodeRift.Managers
{
    public sealed class AudioManager
    {
        private static readonly AudioManager _instance = new AudioManager();
        private readonly Dictionary<string, byte[]> _sounds = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _sfx = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.SFX_JUMP, @"Assets\Audio\sfx\jump.wav" },
            { Constants.SFX_CLICK, @"Assets\Audio\sfx\mouse click.mp3" },
            { Constants.SFX_HOVER, @"Assets\Audio\sfx\hoverbtnsfx.mp3" },
            { Constants.SFX_HIT, @"Assets\Audio\sfx\hit.wav" },
            { Constants.SFX_DEATH, @"Assets\Audio\sfx\death.wav" }
        };
        private readonly Dictionary<string, string> _music = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.MUSIC_MENU, @"Assets\Audio\music\MainMenuBGMusic.mp3" },
            { Constants.MUSIC_PROLOGUE, @"Assets\Audio\music\PrologueBGMusic.mp3" },
            { Constants.MUSIC_LEVELS, @"Assets\Audio\music\LevelsBGMusic.mp3" },
            { Constants.MUSIC_EPILOGUE, @"Assets\Audio\music\EpilogueBGMusic.mp3" }
        };
        private readonly Dictionary<string, string> _cgAudio = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.SFX_CG_CLICK, @"Assets\Audio\sfx\cg_click.wav" },
            { Constants.SFX_CG_END, @"Assets\Audio\sfx\cg_end.wav" },
            { Constants.SFX_CG_ENTER, @"Assets\Audio\sfx\cg_enter.wav" }
        };

        private string _currentMusicKey;
        private int _globalVolume = 800; // Default 80% (0-1000 scale for MCI)
        private bool _sfxEnabled = true;

        public int VolumePercent { get { return _globalVolume / 10; } }
        public bool IsSFXEnabled
        {
            get { return _sfxEnabled; }
            set { _sfxEnabled = value; }
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        private AudioManager()
        {
        }

        public static AudioManager Instance { get { return _instance; } }

        public Dictionary<string, string> SFX { get { return _sfx; } }

        public Dictionary<string, string> Music { get { return _music; } }

        public Dictionary<string, string> CGAudio { get { return _cgAudio; } }

        public IEnumerable<KeyValuePair<string, string>> AllAudio { get { return SFX
            .Concat(Music)
            .Concat(CGAudio); } }

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
            byte[] bytes;
            if (_sounds.TryGetValue(key, out bytes))
            {
                Task.Run(() =>
                {
                    using (MemoryStream stream = new MemoryStream(bytes, writable: false))
                    using (SoundPlayer player = new SoundPlayer(stream))
                    {
                        player.PlaySync();
                    }
                });
                return;
            }

            // Case 2: File-based SFX (e.g. MP3)
            string path = null;
            if (!_sfx.TryGetValue(key, out path))
            {
                if (!_cgAudio.TryGetValue(key, out path)) return;
            }

            string resolvedPath = ResolvePath(path);
            if (!File.Exists(resolvedPath)) return;

            Task.Run(() => 
            {
                string alias = string.Format("SFX_{0}_{1}", key, Guid.NewGuid().ToString("N"));
                mciSendString(string.Format("open \"{0}\" type mpegvideo alias {1}", resolvedPath, alias), null, 0, IntPtr.Zero);
                mciSendString(string.Format("setaudio {0} volume to {1}", alias, _globalVolume), null, 0, IntPtr.Zero);
                mciSendString(string.Format("play {0} wait", alias), null, 0, IntPtr.Zero);
                mciSendString(string.Format("close {0}", alias), null, 0, IntPtr.Zero);
            });
        }

        public void PlayMusic(string key, bool loop = true)
        {
            if (_currentMusicKey == key) return;

            StopMusic();

            string path = null;
            if (!_music.TryGetValue(key, out path)) return;

            string resolvedPath = ResolvePath(path);
            if (!File.Exists(resolvedPath)) return;

            _currentMusicKey = key;
            
            string command = string.Format("open \"{0}\" type mpegvideo alias MyMusic", resolvedPath);
            mciSendString(command, null, 0, IntPtr.Zero);
            
            // Apply current global volume to the new track
            mciSendString(string.Format("setaudio MyMusic volume to {0}", _globalVolume), null, 0, IntPtr.Zero);

            command = "play MyMusic" + (loop ? " repeat" : "");
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void SetVolume(int volumePercent)
        {
            // MCI volume is 0 to 1000
            _globalVolume = Math.Min(1000, Math.Max(0, volumePercent * 10));
            
            if (_currentMusicKey != null)
            {
                mciSendString(string.Format("setaudio MyMusic volume to {0}", _globalVolume), null, 0, IntPtr.Zero);
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
            return Path.IsPathRooted(normalizedPath) ? normalizedPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, normalizedPath);
        }
    }
}
