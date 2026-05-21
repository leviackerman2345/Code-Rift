# Code Rift Defense Cheat Sheet

## 1) 30-Second System Overview
- `Code Rift` is a C# WinForms educational RPG prototype.
- Startup flow is: `Program -> Form1 (splash preload) -> menu -> Prologue -> Levels -> Battle -> Epilogue -> Credits`.
- We use shared managers for common services (`ImageManager`, `LanguageManager`, `ProgressManager`).

## 2) Where To Point In Code
- Entry/startup: `CodeRift/Program.cs`
- Splash + preload + transition: `CodeRift/Form1.cs`
- Asset preload registry: `CodeRift/Core/AssetBootstrapper.cs`
- Main menu routing: `CodeRift/Forms/menu.cs`
- Story progression: `CodeRift/Forms/PrologueForm.cs`
- Level gating: `CodeRift/Forms/LevelsMenuForm.cs`
- Battle UI shell: `CodeRift/Forms/BattleArenaForm.cs`
- Ending flow: `CodeRift/Forms/EpilogueForm.cs`
- Credits screen: `CodeRift/Forms/credits.cs`
- Localization manager: `CodeRift/Managers/LanguageManager.cs`
- Progress manager: `CodeRift/Managers/ProgressManager.cs`

## 3) 5-Minute Script
1. Goal:
Teach C# ideas through a story-based RPG flow.
2. Architecture:
WinForms screens handle UI and navigation; shared logic is in managers and feature helpers.
3. Startup:
`Form1` preloads assets asynchronously, shows loading progress, then opens menu.
4. Game flow:
Menu routes player into story and levels.
5. Progression:
`ProgressManager` controls unlock state.
6. Localization:
`LanguageManager` loads JSON strings (`en.json`, `ph.json`).
7. End flow:
Epilogue opens credits, then returns back to menu path.

## 4) Strong Q&A Template
- Current behavior: what it does now.
- Reason: why this implementation was chosen.
- Limitation: what is still weak.
- Next step: what refactor is planned.

## 5) Likely Panel Questions (Short Answers)
1. Why WinForms?
Fast desktop prototyping and clear event-driven UI.
2. Why preload assets in splash?
To avoid lag during scene changes and show startup progress.
3. How do you control level unlocking?
`ProgressManager` drives button enabled/disabled states.
4. How is localization done?
`LanguageManager` reads JSON files and forms re-apply labels.
5. What are known limitations?
Some logic is still in forms; battle mechanics are still basic.
6. What would you improve first?
Move more non-UI logic into feature classes and add navigation service.

## 6) Honest Limitations To State
- Some forms still mix UI and flow logic.
- Persistence for settings/progress can be extended.
- Architecture migration is in progress feature-by-feature.

## 7) If Asked "What Did You Learn?"
- Building a complete game flow under constraints.
- Managing state transitions in WinForms.
- Refactoring from quick prototype style to cleaner structure.
