# ⚔️ Code Rift: 

**Code Rift** is a story-driven educational RPG built in C# WinForms. Players take on the role of **Elias**, an apprentice of the Syntax Sanctum, as he journeys through a world corrupted by nonsensical code to defeat the **Null King** and restore the **Great Compiler**.

---

## 📖 The Story

In the age before the collapse, the world was a lattice of perfect logic. But a shadow fell across the source code—the **Null King**. Corruption spread, rewriting reality into jagged arrays and infinite loops. Elias, armed with a terminal forged from pure logic, is the world's last hope. He must traverse the **Loop Plains**, climb the **Method Mountains**, brave the **String Seas**, and cross the **Array Abyss** to debug the world, one line at a time.

---

## 🎮 Gameplay Mechanics

Code Rift challenges your C# knowledge through a high-stakes battle system:

- **Logic Units (Battles):** Each level consists of 5 critical logic units (questions).
- **Execution-Based Combat:** There are no multiple-choice safety nets. You must type the correct C# keywords, syntax, or logic into your terminal to strike.
- **Strategic Health Logic:**
  - **Success:** A correct answer deals **20% (1/5th)** damage to the enemy.
  - **Failure:** An incorrect answer causes Elias to take **20% (1/5th)** damage from the Bug.
- **Victory Condition:** To survive a level and progress, Elias must answer **at least 3 out of 5** logic units correctly.

---

## ⌨️ Controls & Key Binds

Interact with the Code Rift terminal using these optimized key binds:

| Key Bind | Action |
| :--- | :--- |
| **Enter** | **Execute Attack:** Submit the code typed in the terminal. |
| **Shift + Enter** | **Line Break:** Move to a new line in multi-line debug scenarios. |
| **Any Key** | **Continue:** Progress past the Splash Screen. |
| **Mouse Click** | **Navigation:** Interact with buttons for Levels, Next/Back in Story, and Main Menu. |

---

## 📚 Educational Content

The game progressively teaches and tests the following C# fundamentals:

1. **Level 1 — Loop Plains:** `for`, `while`, `do-while`, `break`, and `continue`.
2. **Level 2 — Method Mountains:** Method signatures, return types, parameters, and static members.
3. **Level 3 — String Seas:** String properties, manipulation methods (`ToUpper`, `Trim`), and indexing.
4. **Level 4 — Array Abyss:** Indexing, length, initialization, and fixed-size constraints.
5. **Level 5 — The Null King:** Final compilation testing all previous concepts and advanced exceptions.

---

## 🛠️ Technical Details

- **Framework:** .NET 8.0 Windows (WinForms)
- **Architecture:** 
  - Pure programmatic UI (No `.Designer.cs` files).
  - Singleton `GameManager` for state and progress persistence.
  - JSON-based save system (`progress.json` in AppData).
- **Visuals:** High-contrast terminal-green aesthetic (`#00FF41` on `#0D0D0D`).

---

## 🚀 How to Run

1. Open `CodeRift.sln` in **Visual Studio 2022**.
2. Ensure you have the **.NET 8 SDK** installed.
3. Press **F5** to compile and launch the terminal.

*May your syntax be clean and your logic remain unhandled.*
