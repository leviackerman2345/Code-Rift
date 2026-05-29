<div align="center">

# CodeRift

**An educational quiz-battle game with a cyberpunk terminal aesthetic.**

Built with .NET Framework 4.8 WinForms

</div>

---

## Overview

CodeRift is a single-player educational game that combines programming quizzes with turn-based combat. Players progress through five levels, each featuring a unique enemy, answering timed questions to deal damage and advance the storyline. The game features animated sprite-based battles, a visual-novel narrative engine, and full bilingual support (English and Filipino).

---

## Features

- **Turn-Based Quiz Combat** - Select attack cards and answer programming questions to deal damage. Wrong answers lock your card and trigger enemy retaliation.
- **Two Question Modes** - Multiple choice (A/B/C/D) and code input, with per-level time limits ranging from 35 to 20 seconds.
- **Animated Battle System** - Frame-based sprite animations for idle, attack, hurt, and run states with screen shake effects and background tinting.
- **Five Unique Levels** - Progress through Levels 1-5 (Loops, Methods, Strings, Arrays, Classes), each with a distinct enemy and scaling difficulty.
- **Visual Novel Engine** - Reusable story system with typewriter text, scene transitions, and click-to-advance dialogue for prologue and epilogue sequences.
- **Progress Tracking** - Persistent level completion state with unlock gating.
- **Bilingual Localization** - Full English and Filipino language support across all UI and question content.
- **Async Asset Preloading** - Background prewarming of sprite frames and assets during the splash screen for zero-lag transitions.

---

## Project Structure

```
Code-Rift/
├── Program.cs                          # Application entry point
├── Form1.cs                            # Splash screen with async asset loading
├── Forms/
│   ├── MenuForm.cs                     # Main menu hub
│   ├── StoryForm.cs                    # Reusable visual-novel display engine
│   ├── StoryConfig.cs                  # Story data model (StoryStep, StoryConfig)
│   ├── StoryScripts.cs                 # Prologue and epilogue script definitions
│   ├── LevelsMenuForm.cs               # Level selection with unlock states
│   ├── BattleArenaForm.cs              # Main battle form with animation engine
│   ├── BattleArenaQuestionForm.cs      # Quiz modal (multiple choice / code input)
│   ├── BattleLoaderForm.cs             # Pre-battle loading with sprite prewarming
│   ├── FinalVentForm.cs                # Win/lose result screen
│   ├── TerminalMessageBox.cs           # Styled terminal dialog
│   ├── settings.cs                     # Settings (language, volume, SFX)
│   └── credits.cs                      # Credits screen
├── Core/
│   ├── AssetBootstrapper.cs            # Asset preload registry
│   ├── AssetLoadProgress.cs            # Loading progress DTO
│   ├── Battle/
│   │   ├── QuizBattleEngine.cs         # Pure battle logic (HP, cards, damage)
│   │   ├── BattleActorController.cs    # Abstract animated character controller
│   │   ├── PlayerActorController.cs    # Player character actor
│   │   ├── EnemyActorController.cs     # Enemy character actor
│   │   └── LevelConfig.cs             # Per-level enemy configuration
│   ├── Questions/
│   │   └── QuestionSkipCommand.cs      # Skip command parser (/// and /////)
│   └── Transitions/
│       └── FormTransitionManager.cs    # Form fade transition manager
├── Entities/
│   └── Question.cs                     # Question data model
├── Managers/
│   ├── AudioManager.cs                 # Background music and SFX (MCI API)
│   ├── ImageManager.cs                 # Image caching and loading
│   ├── LanguageManager.cs              # Localization string manager
│   ├── Questions/
│   │   └── QuestionManager.cs          # Question bank loader
│   └── Progress/
│       ├── ProgressManager.cs          # Level unlock/completion tracking
│       └── ProgressData.cs             # Save data model
└── Utils/
    ├── Constants.cs                    # Central constants (images, audio, strings)
    ├── AssetPathHelper.cs              # Asset file path resolution
    ├── MenuButtonStyle.cs              # Cyberpunk button styling
    ├── questions.json                  # Quiz questions for all 5 levels
    ├── en.json                         # English localization strings
    └── ph.json                         # Filipino localization strings
```

---

## Game Flow

```
Splash Screen
    └─► Main Menu
            ├─► Prologue (StoryForm)
            │       └─► Level Select
            │               └─► Battle Loader
            │                       └─► Battle Arena
            │                               ├─► Question Modal (per turn)
            │                               └─► Final Vent (win/lose)
            │                                       └─► Epilogue (StoryForm, Level 5 only)
            ├─► Settings
            └─► Credits
```

---

## Battle System

Each level pits the player against an enemy in a card-based quiz duel:

| Mechanic | Description |
|----------|-------------|
| **HP** | Both player and enemy start at 100 HP |
| **Attack Cards** | 5 cards dealing 10/15/20/25/30 damage respectively |
| **Correct Answer** | Selected card's damage is applied to the enemy |
| **Wrong Answer** | Card becomes locked; enemy retaliates with chain attacks |
| **Locked Card** | Must retry the locked card before using any other card |
| **Idle Timer** | 15-second inactivity timer deals 5 chip damage to the player |
| **Skip Commands** | Type `///` to skip current question, `/////` to skip all remaining |

---

## Requirements

- **OS:** Windows 10/11
- **Runtime:** .NET Framework 4.8
- **IDE:** Visual Studio 2019 or later (for development)

---

## Getting Started

1. Open `Code-Rift.sln` in Visual Studio.
2. Restore NuGet packages (Newtonsoft.Json).
3. Build and run the project (`F5`).

---

## Localization

The game supports English and Filipino. Localization strings are stored in:

- `Utils/en.json` - English
- `Utils/ph.json` - Filipino

Language can be toggled in the Settings screen at runtime.

---

<div align="center">

**Excluded from review:** `*.Designer.cs` and `*.resx` (auto-generated WinForms files)

</div>
