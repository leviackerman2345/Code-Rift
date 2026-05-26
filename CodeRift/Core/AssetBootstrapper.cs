using CodeRift.Managers;
using CodeRift.Utils;
using System.IO;

namespace CodeRift.Core
{
    // Central preload registry used by splash screen for deterministic startup.
    public static class AssetBootstrapper
    {
        public const string SplashBackgroundKey = "__SPLASH_BACKGROUND__";
        public const string SplashTitleKey = "__SPLASH_TITLE__";

        private static readonly (string Key, string Path)[] ImageAssets =
        {
            (Constants.IMG_BG_MENU, Path.Combine("Assets", "Images", "backgrounds", "main_menu.png")),
            (Constants.IMG_BG_LEVEL1, Path.Combine("Assets", "Images", "backgrounds", "level_background", "level_1.png")),
            (Constants.IMG_BG_LEVEL2, Path.Combine("Assets", "Images", "backgrounds", "level_background", "level_2.png")),
            (Constants.IMG_BG_LEVEL3, Path.Combine("Assets", "Images", "backgrounds", "level_background", "level_3.png")),
            (Constants.IMG_BG_LEVEL4, Path.Combine("Assets", "Images", "backgrounds", "level_background", "level_4.png")),
            (Constants.IMG_BG_LEVEL5, Path.Combine("Assets", "Images", "backgrounds", "level_background", "level_5.png")),
            (Constants.IMG_UI_BUTTON, Path.Combine("Assets", "Images", "ui", "button_hover.png")),
            (Constants.IMG_UI_DIALOGUE, Path.Combine("Assets", "Images", "ui", "dialogue_box.png")),
            (Constants.CG_01, Path.Combine("Assets", "Images", "prologue", "scene_1.jpeg")),
            (Constants.CG_02, Path.Combine("Assets", "Images", "prologue", "scene_2.jpeg")),
            (Constants.CG_03, Path.Combine("Assets", "Images", "prologue", "scene_3.jpeg")),
            (Constants.CG_04, Path.Combine("Assets", "Images", "prologue", "scene_4.jpeg")),
            (Constants.CG_05, Path.Combine("Assets", "Images", "prologue", "scene_5.jpeg")),
            (Constants.CG_06, Path.Combine("Assets", "Images", "prologue", "scene_6.jpeg")),
            (Constants.CG_07, Path.Combine("Assets", "Images", "prologue", "scene_7.jpeg")),
            (Constants.CG_08, Path.Combine("Assets", "Images", "prologue", "scene_8.jpeg")),
            (Constants.CG_09, Path.Combine("Assets", "Images", "prologue", "scene_9.jpeg")),
            (Constants.CG_10, Path.Combine("Assets", "Images", "prologue", "scene_10.jpeg")),
            (Constants.CG_11, Path.Combine("Assets", "Images", "prologue", "scene_11.jpeg")),
            (Constants.CG_12, Path.Combine("Assets", "Images", "prologue", "scene_12.jpeg")),
            (Constants.CG_13, Path.Combine("Assets", "Images", "prologue", "scene_13.jpeg")),
            (Constants.EP_01, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_1.png")),
            (Constants.EP_02, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_2.png")),
            (Constants.EP_03, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_3.png")),
            (Constants.EP_04, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_4.png")),
            (Constants.EP_05, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_5.png")),
            (Constants.EP_06, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_6.png")),
            (Constants.EP_07, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_7.png")),
            (Constants.EP_08, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_8.png")),
            (Constants.EP_09, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_9.png")),
            (Constants.EP_10, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_10.png")),
            (Constants.EP_11, Path.Combine("Assets", "Images", "epilogue", "epilogue_img_11.png")),
            (SplashBackgroundKey, Path.Combine("Assets", "Images", "backgrounds", "Splash background.jpeg")),
            (SplashTitleKey, Path.Combine("Assets", "Images", "ui", "Title.png"))
        };

        private static readonly (string Key, string Path)[] AudioAssets =
        {
            (Constants.SFX_CLICK, Path.Combine("Assets", "Audio", "sfx", "mouse click.mp3")),
            (Constants.SFX_HOVER, Path.Combine("Assets", "Audio", "sfx", "hoverbtnsfx.mp3")),
            (Constants.SFX_CG_CLICK, Path.Combine("Assets", "Audio", "sfx", "cg_click.wav")),
            (Constants.SFX_CG_END, Path.Combine("Assets", "Audio", "sfx", "cg_end.wav")),
            (Constants.SFX_CG_ENTER, Path.Combine("Assets", "Audio", "sfx", "cg_enter.wav")),
            (Constants.MUSIC_MENU, Path.Combine("Assets", "Audio", "music", "MainMenuBGMusic.mp3")),
            (Constants.MUSIC_PROLOGUE, Path.Combine("Assets", "Audio", "music", "PrologueBGMusic.mp3")),
            (Constants.MUSIC_LEVELS, Path.Combine("Assets", "Audio", "music", "LevelsBGMusic.mp3")),
            (Constants.MUSIC_EPILOGUE, Path.Combine("Assets", "Audio", "music", "EpilogueBGMusic.mp3"))
        };

        public static async Task LoadAllAsync(IProgress<AssetLoadProgress>? progress = null, CancellationToken cancellationToken = default)
        {
            // Language first so loading messages are localized.
            LanguageManager.Instance.Load(Constants.LANG_EN);

            int total = ImageAssets.Length + AudioAssets.Length;
            int loaded = 0;

            await Task.Run(() =>
            {
                // Preload image resources into ImageManager cache.
                foreach (var asset in ImageAssets)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    ImageManager.Instance.LoadImage(asset.Key, asset.Path);
                    loaded++;
                    progress?.Report(new AssetLoadProgress
                    {
                        LoadedCount = loaded,
                        TotalCount = total,
                        AssetName = asset.Key,
                        Message = BuildMessage(asset.Path)
                    });
                }

                // Audio section is currently prepared for expansion (array may be empty).
                foreach (var asset in AudioAssets)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    AudioManager.Instance.LoadSound(asset.Key, asset.Path);
                    loaded++;
                    progress?.Report(new AssetLoadProgress
                    {
                        LoadedCount = loaded,
                        TotalCount = total,
                        AssetName = asset.Key,
                        Message = BuildMessage(asset.Path)
                    });
                }
            }, cancellationToken);

            progress?.Report(new AssetLoadProgress
            {
                LoadedCount = total,
                TotalCount = total,
                AssetName = "Battle Sprites",
                Message = LanguageManager.Instance.Get("loading") + " Battle Data..."
            });

            // Preload all battle sprites at the splash screen to guarantee completely 
            // instant level loads and zero background CPU starvation during the game.
            await CodeRift.Forms.BattleArenaForm.PrewarmAllLevelsAsync();

            progress?.Report(new AssetLoadProgress
            {
                LoadedCount = total,
                TotalCount = total,
                AssetName = string.Empty,
                Message = LanguageManager.Instance.Get("loading_done")
            });
        }

        public static void LoadSplashArtwork()
        {
            // Lightweight preload so splash can render before full asset pass completes.
            ImageManager.Instance.LoadImage(SplashBackgroundKey, Path.Combine("Assets", "Images", "backgrounds", "Splash background.jpeg"));
            ImageManager.Instance.LoadImage(SplashTitleKey, Path.Combine("Assets", "Images", "ui", "Title.png"));
        }

        private static string BuildMessage(string path)
        {
            string resolved = Path.Combine(AppContext.BaseDirectory, path);
            return File.Exists(resolved)
                ? $"{LanguageManager.Instance.Get("loading")} {path}"
                : $"Missing: {path}";
        }
    }
}
