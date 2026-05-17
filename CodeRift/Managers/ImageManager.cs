using CodeRift.Utils;
using System.Drawing;
using System.IO;

namespace CodeRift.Managers
{
    public sealed class ImageManager
    {
        private static readonly ImageManager _instance = new ImageManager();
        private readonly Dictionary<string, Image> _images = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _backgrounds = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.IMG_BG_MENU, @"Assets\Images\backgrounds\main_menu.png" },
            { Constants.IMG_BG_LEVEL1, @"Assets\Images\backgrounds\level1.png" },
            { Constants.IMG_BG_LEVEL2, @"Assets\Images\backgrounds\level2.png" }
        };
        private readonly Dictionary<string, string> _playerSprites = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.IMG_PLAYER_IDLE, @"Assets\Images\player\idle.png" },
            { Constants.IMG_PLAYER_RUN, @"Assets\Images\player\run.png" },
            { Constants.IMG_PLAYER_JUMP, @"Assets\Images\player\jump.png" },
            { Constants.IMG_PLAYER_ATTACK, @"Assets\Images\player\attack.png" }
        };
        private readonly Dictionary<string, string> _enemySprites = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.IMG_ENEMY_BASIC, @"Assets\Images\enemies\basic.png" },
            { Constants.IMG_ENEMY_BOSS, @"Assets\Images\enemies\boss.png" }
        };
        private readonly Dictionary<string, string> _uiSprites = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.IMG_UI_BUTTON, @"Assets\Images\ui\button_hover.png" },
            { Constants.IMG_UI_HEALTHBAR, @"Assets\Images\ui\healthbar.png" },
            { Constants.IMG_UI_DIALOGUE, @"Assets\Images\ui\dialogue_box.png" }
        };
        private readonly Dictionary<string, string> _cgImages = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.CG_01, @"Assets\Images\prologue\scene_1.jpeg" },
            { Constants.CG_02, @"Assets\Images\prologue\scene_2.jpeg" },
            { Constants.CG_03, @"Assets\Images\prologue\scene_3.jpeg" },
            { Constants.CG_04, @"Assets\Images\prologue\scene_4.jpeg" },
            { Constants.CG_05, @"Assets\Images\prologue\scene_5.jpeg" },
            { Constants.CG_06, @"Assets\Images\prologue\scene_6.jpeg" },
            { Constants.CG_07, @"Assets\Images\prologue\scene_7.jpeg" },
            { Constants.CG_08, @"Assets\Images\prologue\scene_8.jpeg" },
            { Constants.CG_09, @"Assets\Images\prologue\scene_9.jpeg" },
            { Constants.CG_10, @"Assets\Images\prologue\scene_10.jpeg" },
            { Constants.CG_11, @"Assets\Images\prologue\scene_11.jpeg" },
            { Constants.CG_12, @"Assets\Images\prologue\scene_12.jpeg" },
            { Constants.CG_13, @"Assets\Images\prologue\scene_13.jpeg" }
        };

        private ImageManager()
        {
        }

        public static ImageManager Instance => _instance;

        public IReadOnlyDictionary<string, string> Backgrounds => _backgrounds;

        public IReadOnlyDictionary<string, string> PlayerSprites => _playerSprites;

        public IReadOnlyDictionary<string, string> EnemySprites => _enemySprites;

        public IReadOnlyDictionary<string, string> UISprites => _uiSprites;

        public IReadOnlyDictionary<string, string> CGImages => _cgImages;

        public IEnumerable<KeyValuePair<string, string>> AllImages => Backgrounds
            .Concat(PlayerSprites)
            .Concat(EnemySprites)
            .Concat(UISprites)
            .Concat(CGImages);

        public void LoadImage(string key, string path)
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

            byte[] bytes = File.ReadAllBytes(resolvedPath);
            using MemoryStream stream = new MemoryStream(bytes, writable: false);
            using Image loadedImage = Image.FromStream(stream);
            Image image = new Bitmap(loadedImage);

            if (_images.TryGetValue(key, out Image? existingImage))
            {
                existingImage.Dispose();
            }

            _images[key] = image;
        }

        public Image? GetImage(string key)
        {
            return _images.TryGetValue(key, out Image? image) ? image : null;
        }

        public void Unload()
        {
            foreach (Image image in _images.Values)
            {
                image.Dispose();
            }

            _images.Clear();
        }

        private static string ResolvePath(string path)
        {
            string normalizedPath = path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            return Path.IsPathRooted(normalizedPath) ? normalizedPath : Path.Combine(AppContext.BaseDirectory, normalizedPath);
        }
    }
}
