# Credits Defense Guide (Beginner-Friendly)

## One-Line Explanation
The `CreditsForm` is both the screen and the logic holder for now, to keep beginner defense simple.

## Simple 2-Box Model
- Box 1: Screen + Logic (WinForms)
  - `CodeRift/Forms/credits.cs`
  - Contains crawl animation, render math, content list, and asset loading.
- Box 2: Designer/UI structure
  - `CodeRift/Forms/credits.Designer.cs`
  - `CodeRift/Forms/credits.resx`

## 30-Second Defense Script
We kept `credits` as a real WinForms form because UI screens must be forms.
For now, we put logic directly in `credits.cs` so beginners can explain everything from one file.
Later, it can be split again when the team is ready.

## If Panel Asks "Why Not Keep Everything In Form?"
For this stage, one-file logic is easier to present and defend.
When complexity grows, we can split again into helper classes.

## If Panel Asks "Is This Over-Engineering?"
No. This is intentional for beginner clarity.
It is a temporary simplification, not a technical limitation.

## If Panel Asks "What Did You Improve Exactly?"
- Kept one real form file for credits.
- Put crawl logic directly in `credits.cs`.
- Preserved same visual output and controls.
