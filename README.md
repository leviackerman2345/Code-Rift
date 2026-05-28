<div align="center">

# CodeRift

### Reviewer File Guide

An educational quiz-battle game built with **.NET Framework 4.8 WinForms** featuring a cyberpunk terminal aesthetic.

</div>

---

## Presentation Order

| # | Member | Topic |
|:-:|--------|-------|
| 1 | Member 1 | Splash Screen and Main Menu |
| 2 | Member 2 | Reusable StoryForm (Prologue & Epilogue) |
| 3 | Member 3 | Level Menu |
| 4 | Member 4 | Battle Arena |
| 5 | Member 5 | Question Form and Final Vent |

---

## File Guide

### 1 - Splash Screen and Main Menu

> Loading screen with async asset preloading, progress bar, and fade transitions into the main menu hub.

| File | Description |
|------|-------------|
| `Form1.cs` | Splash screen with async loading, progress bar, fade animation |
| `Forms/menu.cs` | Main menu hub (Play, Levels, Settings, Credits, Exit) |
| `Core/AssetBootstrapper.cs` | Preload registry for all game assets |
| `Core/AssetLoadProgress.cs` | Progress DTO for splash loading feedback |
| `Core/Transitions/FormTransitionManager.cs` | Splash-to-menu fade transition manager |
| `Program.cs` | Application entry point |
| `Managers/AudioManager.cs` | Background music playback |
| `Managers/ImageManager.cs` | Image caching and loading |
| `Managers/LanguageManager.cs` | Localized button labels |
| `Utils/MenuButtonStyle.cs` | Cyberpunk button styling |

---

### 2 - Reusable StoryForm (Prologue & Epilogue)

> Visual-novel-style engine with typewriter text, scene fades, and click-to-advance. One form, two stories.

| File | Description |
|------|-------------|
| `Forms/StoryForm.cs` | Reusable visual-novel display engine |
| `Forms/StoryConfig.cs` | Data model (`StoryStep`, `StoryConfig`) |
| `Forms/StoryScripts.cs` | `CreatePrologue()` (49 steps) and `CreateEpilogue()` (24 steps) |
| `Core/Transitions/FormTransitionManager.cs` | Transitions into/out of StoryForm |
| `Utils/Constants.cs` | CG image keys (`CG_01`-`CG_13`, `EP_01`-`EP_11`) |

---

### 3 - Level Menu

> Level selection with unlock state tracking, background crossfade on hover, and locked-level warnings.

| File | Description |
|------|-------------|
| `Forms/LevelsMenuForm.cs` | 5 level buttons with crossfade and unlock state |
| `Managers/Progress/ProgressManager.cs` | Level unlock/progress tracking |
| `Managers/Progress/ProgressData.cs` | Save data POCO |
| `Utils/MenuButtonStyle.cs` | Normal and locked button styles |
| `Forms/TerminalMessageBox.cs` | "Level Locked" warning dialog |

---

### 4 - Battle Arena

> Animated sprite-based combat with HP bars, card selection, idle timer, screen shake, and enemy AI.

| File | Description |
|------|-------------|
| `Forms/BattleArenaForm.cs` | Main battle form (~1920 lines) with full animation engine |
| `Forms/BattleLoaderForm.cs` | Pre-battle loading screen with sprite prewarming |
| `Core/Battle/QuizBattleEngine.cs` | Pure battle logic (HP, cards, damage, chaining) |
| `Core/Battle/BattleActorController.cs` | Abstract animated character controller |
| `Core/Battle/PlayerActorController.cs` | Player character actor |
| `Core/Battle/EnemyActorController.cs` | Enemy character actor |
| `Core/Battle/LevelConfig.cs` | Per-level enemy configuration (5 enemies) |
| `Entities/Question.cs` | Question data model |
| `Managers/Questions/QuestionManager.cs` | Question bank from `questions.json` |

---

### 5 - Question Form and Final Vent

> Terminal-styled quiz modal with countdown timer, multiple input modes, and post-battle result screens.

| File | Description |
|------|-------------|
| `Forms/BattleArenaQuestionForm.cs` | Question modal (MultipleChoice / CodeInput, timer, skip commands) |
| `Forms/FinalVentForm.cs` | Win/lose result screen with level-specific images |
| `Core/Questions/QuestionSkipCommand.cs` | Parses `///` and `/////` skip commands |
| `Entities/Question.cs` | Question data model |
| `Managers/Questions/QuestionManager.cs` | Question bank loader |

---

## Shared Files

Used across multiple features:

| File | Role |
|------|------|
| `Utils/Constants.cs` | Central image/audio/string constants |
| `Utils/AssetPathHelper.cs` | Asset file path resolution |
| `Managers/AudioManager.cs` | Audio playback (MCI API) |
| `Managers/ImageManager.cs` | Image caching |
| `Managers/LanguageManager.cs` | EN/PH localization |
| `Core/Transitions/FormTransitionManager.cs` | Form fade transitions |

---

## Data Files

| File | Purpose |
|------|---------|
| `Utils/questions.json` | All quiz questions for 5 levels |
| `Utils/en.json` | English localization strings |
| `Utils/ph.json` | Filipino localization strings |

---

<div align="center">

**Excluded:** `*.Designer.cs` and `*.resx` (auto-generated WinForms files)

</div>
