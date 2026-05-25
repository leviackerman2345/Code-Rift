# Code Rift

Code Rift is a WinForms C# game built around terminal-style menus, story screens, and quiz battles.

## Overview

The game follows this flow:

1. Splash/loading screen preloads assets.
2. Main menu opens.
3. Prologue story plays.
4. Level menu shows unlocked levels and hover background previews.
5. Battle arena runs quiz-based fights.
6. Final Vent screen appears after battle results.
7. Epilogue plays on the win path.
8. Credits run automatically and return to the main menu.

## Current Features

- Splash screen with async asset loading and fade transition.
- Reusable story system for Prologue and Epilogue.
- Level menu with smooth background crossfade on hover.
- Progress-based level unlocking.
- Battle arena with enemy configs, animation loading, and reusable battle flow.
- Global skip commands in question input:
  - `///` skips the current question and performs the normal attack flow.
  - `/////` skips the remaining questions and applies exactly 100 damage once.
- Final Vent screen with reusable image loading.
- Mandatory credits sequence that automatically returns to the main menu.
- Reusable button styling across forms.
- Asset caching through the shared loader.

## Project Structure

```text
CodeRift/
├── Assets/
│   ├── Audio/
│   │   ├── music/
│   │   └── sfx/
│   └── Images/
│       ├── backgrounds/
│       ├── enemies/
│       ├── epilogue/
│       ├── player/
│       ├── prologue/
│       └── ui/
├── Core/
├── Entities/
├── Forms/
├── Managers/
└── Utils/
```

## Main Systems

- `Core/` contains battle logic, asset bootstrapping, and transition helpers.
- `Entities/` contains data models such as questions and story content.
- `Forms/` contains the splash screen, menu, level menu, battle arena, story form, final vent, credits, and question UI.
- `Managers/` contains asset, audio, language, progress, and question loading systems.
- `Utils/` contains shared constants and question data.

## Build

- Target framework: `net10.0-windows`
- UI framework: `Windows Forms`
- Nullable reference types: enabled

## Run

- Open the solution in Visual Studio 2022 or newer, then run `CodeRift`.
- Or run from the project folder with:

```bash
dotnet run --project CodeRift/CodeRift.csproj
```

The app starts at the splash/loading screen (`Form1`) and then fades into the main menu.

## Controls

- Menu and story screens: mouse click to advance or select options.
- Level menu: hover a level button to preview its background, click an unlocked level to enter.
- Battle question screens:
  - `Enter` submits the current answer.
  - `Esc` goes back.
  - `A`, `B`, `C`, `D` select multiple-choice answers.
  - Type `///` to skip the current question.
  - Type `/////` to skip the rest of the sequence and deal 100 damage once.

## Notes

- Assets are loaded through the shared loader instead of directly from UI code.
- Battle content is driven by reusable config classes so future levels and enemies can be added without hardcoding screen logic.
- The project uses fade transitions to reduce flicker between screens.
