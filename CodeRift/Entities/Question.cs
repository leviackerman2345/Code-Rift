using System.Collections.Generic;

namespace CodeRift.Entities
{
    /// <summary>
    /// Represents the different types of challenges in the game.
    /// </summary>
    public enum QuestionType
    {
        MultipleChoice,
        CodeInput
    }

    /// <summary>
    /// A reusable data container for battle questions.
    /// This allows us to load questions from a file rather than hardcoding them.
    /// </summary>
    public class Question
    {
        // Which level this question belongs to (1-5)
        public int Level { get; set; }

        // The title of the level (e.g. LOOPS, METHODS, etc.)
        public string LevelTitle { get; set; } = string.Empty;

        // The type of interaction required (MC or Code)
        public QuestionType Type { get; set; }

        // The text of the problem or task
        public string ProblemStatement { get; set; } = string.Empty;

        // For Multiple Choice: List of options
        // For Code Input: This can be null
        public List<string>? Options { get; set; }

        // The exact correct string (for Code) or the correct option text (for MC)
        public string CorrectAnswer { get; set; } = string.Empty;

        // An optional hint to display if the player is stuck
        public string Hint { get; set; } = string.Empty;
    }

    /// <summary>
    /// Wrapper for the JSON list of questions
    /// </summary>
    public class QuestionBank
    {
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
