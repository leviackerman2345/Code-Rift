# CodeRift Architecture (Readable Guide)

## 1) Big Picture
CodeRift is a WinForms game app.

- `Program.cs` starts the app.
- `Form1` is the splash/boot loader.
- `MenuForm` is the main hub.
- Feature screens (`Prologue`, `Levels`, `Battle`, `Epilogue`, `Credits`, `Settings`) handle gameplay flow.

## 2) Runtime Flow
1. `Program` -> starts `Form1`.
2. `Form1` -> preloads assets with `AssetBootstrapper`.
3. `Form1` -> opens `MenuForm`.
4. `MenuForm` -> routes to other screens.

## 3) Current Folder Responsibilities
- `Forms/`: UI screens, user interaction, and navigation.
- `Core/`: startup/loading orchestration.
- `Managers/`: shared singleton services (images/audio/language).
- `Utils/`: constants and JSON language resources.
- `Features/`: reserved for future layered modules; currently not used by `Credits`.

## 4) Current Credits Pattern (Beginner-Friendly)
`Credits` is intentionally implemented directly in the form:

- `Forms/credits.cs`
  - crawl animation logic
  - perspective render math
  - credits content list
  - asset loading
- `Forms/credits.Designer.cs` + `Forms/credits.resx`
  - visual layout + form resources

## 5) Dependency Rule (Important)
Use this direction only:

`Forms` -> `Managers/Core/Utils`

Never the reverse.

- Keep forms readable with clear method grouping.
- Move out complex logic only when team is ready.

## 6) What Is Still Legacy-Style
These still mix content + flow + UI in one class:
- `Forms/PrologueForm.cs`
- `Forms/EpilogueForm.cs`
- `Forms/menu.cs`
- `Forms/settings.cs`

## 7) Recommended Migration Order
1. Keep credits in-form for defense simplicity.
2. Improve naming and comments in other forms.
3. Later, migrate one screen at a time to layered feature modules.
4. Add a small navigation service when refactoring resumes.

## 8) Why This Improves Readability
- Smaller classes with one clear purpose.
- Easier onboarding: "where to change what" is obvious.
- Safer changes: UI edits do not break domain logic.
- Better testability for non-UI behavior.
