using CodeRift.Managers;
using CodeRift.Utils;
using System.IO;

namespace CodeRift.Core
{
    public static class AssetBootstrapper
    {
        public const string SplashBackgroundKey = "__SPLASH_BACKGROUND__";
        public const string SplashTitleKey = "__SPLASH_TITLE__";

        private static readonly (string Key, string Path)[] ImageAssets =
        {
            (Constants.IMG_BG_MENU, Path.Combine("Assets", "Images", "backgrounds", "main_menu.png")),
            (Constants.IMG_BG_LEVEL1, Path.Combine("Assets", "Images", "backgrounds", "level1.png")),
            (Constants.IMG_BG_LEVEL2, Path.Combine("Assets", "Images", "backgrounds", "level2.png")),
            (Constants.IMG_PLAYER_IDLE, Path.Combine("Assets", "Images", "player", "idle.png")),
            (Constants.IMG_PLAYER_RUN, Path.Combine("Assets", "Images", "player", "run.png")),
            (Constants.IMG_PLAYER_JUMP, Path.Combine("Assets", "Images", "player", "jump.png")),
            (Constants.IMG_PLAYER_ATTACK, Path.Combine("Assets", "Images", "player", "attack.png")),
            (Constants.IMG_ENEMY_BASIC, Path.Combine("Assets", "Images", "enemies", "basic.png")),
            (Constants.IMG_ENEMY_BOSS, Path.Combine("Assets", "Images", "enemies", "boss.png")),
            (Constants.IMG_UI_BUTTON, Path.Combine("Assets", "Images", "ui", "button_hover.png")),
            (Constants.IMG_UI_HEALTHBAR, Path.Combine("Assets", "Images", "ui", "healthbar.png")),
            ("IMG_UI_DIALOGUE", Path.Combine("Assets", "Images", "ui", "dialogue_box.png")),
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
            (SplashBackgroundKey, Path.Combine("Assets", "Images", "backgrounds", "Splash background.jpeg")),
            (SplashTitleKey, Path.Combine("Assets", "Images", "ui", "Title.png"))
        };

        private static readonly (string Key, string Path)[] AudioAssets =
        {
            (Constants.SFX_JUMP, Path.Combine("Assets", "Audio", "sfx", "jump.wav")),
            (Constants.SFX_CLICK, Path.Combine("Assets", "Audio", "sfx", "click.wav")),
            (Constants.SFX_HIT, Path.Combine("Assets", "Audio", "sfx", "hit.wav")),
            (Constants.SFX_DEATH, Path.Combine("Assets", "Audio", "sfx", "death.wav")),
            (Constants.SFX_CG_CLICK, Path.Combine("Assets", "Audio", "sfx", "cg_click.wav")),
            (Constants.SFX_CG_END, Path.Combine("Assets", "Audio", "sfx", "cg_end.wav")),
            (Constants.SFX_CG_ENTER, Path.Combine("Assets", "Audio", "sfx", "cg_enter.wav")),
            (Constants.MUSIC_MENU, Path.Combine("Assets", "Audio", "music", "menu.wav")),
            (Constants.MUSIC_LEVEL1, Path.Combine("Assets", "Audio", "music", "level1.wav")),
            (Constants.MUSIC_BOSS, Path.Combine("Assets", "Audio", "music", "boss.wav")),
            (Constants.MUSIC_CG_EVENT, Path.Combine("Assets", "Audio", "music", "cg_event.wav"))
        };

        public static async Task LoadAllAsync(IProgress<AssetLoadProgress>? progress = null, CancellationToken cancellationToken = default)
        {
            LanguageManager.Instance.Load(Constants.LANG_EN);

            int total = ImageAssets.Length + AudioAssets.Length;
            int loaded = 0;

            await Task.Run(() =>
            {
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
                AssetName = string.Empty,
                Message = LanguageManager.Instance.Get("loading_done")
            });
        }

        public static void LoadSplashArtwork()
        {
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
