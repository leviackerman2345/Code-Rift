================================================================================
                        CODERIFT - REVIEWER FILE GUIDE
                     Code Files Per Presentation Topic
================================================================================

Presentation Order:
  1. Member 1 - Splash Screen and Main Menu
  2. Member 2 - Reusable StoryForm for Prologue and Epilogue
  3. Member 3 - Level Menu
  4. Member 4 - Battle Arena
  5. Member 5 - Question Form and Final Vent

================================================================================
  MEMBER 1: SPLASH SCREEN AND MAIN MENU
================================================================================

  PRIMARY FORMS:
    Form1.cs                          - Splash screen (async asset loading,
                                        progress bar, fade animation, transition
                                        to main menu)
    Forms/menu.cs                     - Main menu hub (Play, Levels, Settings,
                                        Credits, Exit buttons)

  CORE / SUPPORT:
    Core/AssetBootstrapper.cs         - Preload registry invoked by splash screen
    Core/AssetLoadProgress.cs         - Progress DTO for splash loading feedback
    Core/Transitions/FormTransitionManager.cs
                                      - Manages splash-to-menu fade transition
    Program.cs                        - Application entry point, launches Form1

  MANAGERS:
    Managers/AudioManager.cs          - Menu background music playback
    Managers/ImageManager.cs          - Splash/menu background and title image
                                        loading
    Managers/LanguageManager.cs       - Localized menu button labels

  UTILITIES:
    Utils/Constants.cs                - Splash dimensions, image/audio key
                                        constants
    Utils/MenuButtonStyle.cs          - Shared cyberpunk button styling
    Utils/AssetPathHelper.cs          - Asset file path resolution

  RELATED FORMS (launched from menu):
    Forms/settings.cs                 - Settings modal (opened from menu)
    Forms/credits.cs                  - Credits screen (opened from menu)


================================================================================
  MEMBER 2: REUSABLE STORYFORM FOR PROLOGUE AND EPILOGUE
================================================================================

  PRIMARY FORMS:
    Forms/StoryForm.cs                - Reusable visual-novel display engine
                                        (typewriter text, scene fades, dialogue
                                        box, click-to-advance, Back/Skip buttons)
    Forms/StoryConfig.cs              - Data model (StoryStep and StoryConfig
                                        classes that drive the story)
    Forms/StoryScripts.cs             - Story content factory: CreatePrologue()
                                        (49 steps) and CreateEpilogue() (24 steps)

  CORE / SUPPORT:
    Core/Transitions/FormTransitionManager.cs
                                      - Handles transitions into/out of StoryForm

  MANAGERS:
    Managers/AudioManager.cs          - CG-specific audio and background music
    Managers/ImageManager.cs          - CG scene image loading

  UTILITIES:
    Utils/Constants.cs                - CG_01 through CG_13 (prologue) and
                                        EP_01 through EP_11 (epilogue) image
                                        key constants, audio keys


================================================================================
  MEMBER 3: LEVEL MENU
================================================================================

  PRIMARY FORMS:
    Forms/LevelsMenuForm.cs           - Level selection screen with 5 level
                                        buttons, background crossfade on hover,
                                        unlock state checking, Back button

  MANAGERS:
    Managers/Progress/ProgressManager.cs
                                      - Determines which levels are unlocked
    Managers/Progress/ProgressData.cs - Save data POCO (HighestLevelCompleted)

  UTILITIES:
    Utils/MenuButtonStyle.cs          - Applies normal and locked button styles
    Utils/Constants.cs                - Level background image keys

  SUPPORTING FORMS:
    Forms/TerminalMessageBox.cs       - "Level Locked" warning dialog


================================================================================
  MEMBER 4: BATTLE ARENA
================================================================================

  PRIMARY FORMS:
    Forms/BattleArenaForm.cs          - Main battle form (~1920 lines): sprite
                                        animation engine, HP bars, card selection,
                                        idle timer, screen shake, battle result
                                        handling, epilogue trigger
    Forms/BattleLoaderForm.cs         - Pre-battle loading screen with sprite
                                        prewarming and segmented progress bar

  CORE / BATTLE SYSTEM:
    Core/Battle/QuizBattleEngine.cs   - Pure battle logic (HP, cards, damage,
                                        enemy attack chaining)
    Core/Battle/BattleActorController.cs
                                      - Abstract base class for animated battle
                                        characters (Run/Idle/Attack/Hurt frames)
    Core/Battle/PlayerActorController.cs
                                      - Player character actor
    Core/Battle/EnemyActorController.cs
                                      - Enemy character actor
    Core/Battle/LevelConfig.cs        - Per-level enemy configuration (5 enemies)

  ENTITIES:
    Entities/Question.cs              - Question data model (QuestionType enum,
                                        Question class, QuestionBank)

  MANAGERS:
    Managers/Questions/QuestionManager.cs
                                      - Question bank loaded from questions.json
    Managers/Progress/ProgressManager.cs
                                      - Level completion tracking
    Managers/AudioManager.cs          - Battle SFX and music
    Managers/ImageManager.cs          - Sprite and background loading

  UTILITIES:
    Utils/Constants.cs                - Image/audio key constants
    Utils/AssetPathHelper.cs          - Asset path resolution


================================================================================
  MEMBER 5: QUESTION FORM AND FINAL VENT
================================================================================

  PRIMARY FORMS:
    Forms/BattleArenaQuestionForm.cs  - Terminal-styled question modal:
                                        MultipleChoice (A/B/C/D) and CodeInput
                                        modes, countdown timer, skip commands,
                                        incorrect-answer feedback
    Forms/FinalVentForm.cs            - Post-battle win/lose result screen with
                                        level-specific images

  CORE / SUPPORT:
    Core/Questions/QuestionSkipCommand.cs
                                      - Parses /// and ///// skip commands

  ENTITIES:
    Entities/Question.cs              - Question data model (QuestionType,
                                        Question, QuestionBank)

  MANAGERS:
    Managers/Questions/QuestionManager.cs
                                      - Question bank from questions.json
    Managers/AudioManager.cs          - SFX for question interactions

  UTILITIES:
    Utils/Constants.cs                - Image/audio key constants

  SUPPORTING FORMS:
    Forms/TerminalMessageBox.cs       - Locked-card and incorrect-answer popups
    Forms/BattleArenaForm.cs          - Contains OpenQuestionForm(),
                                        RunPlayerTurn(), and ShowFinalVent()
                                        which orchestrate these flows


================================================================================
  SHARED / PROJECT-WIDE FILES
================================================================================

  These files are used across multiple features:

    Program.cs                        - Entry point
    CodeRift.csproj                   - Project file (.NET Framework 4.8)
    Utils/Constants.cs                - Central constants (used everywhere)
    Utils/AssetPathHelper.cs          - Asset path resolution (used everywhere)
    Managers/AudioManager.cs          - Audio (used by nearly all forms)
    Managers/ImageManager.cs          - Image cache (used by nearly all forms)
    Managers/LanguageManager.cs       - Localization (used by menu, settings)
    Core/Transitions/FormTransitionManager.cs
                                      - Form transitions (used across features)
    Utils/questions.json              - Question data (loaded by QuestionManager)
    Utils/en.json                     - English localization strings
    Utils/ph.json                     - Filipino localization strings

================================================================================
  EXCLUDED FROM THIS GUIDE
================================================================================

  The following file types were excluded per instructions:
    - *.Designer.cs files (auto-generated WinForms designer code)
    - *.resx files (auto-generated resource files)

================================================================================
