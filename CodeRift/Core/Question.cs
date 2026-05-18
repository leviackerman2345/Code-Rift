using System.Collections.Generic;

namespace CodeRift.Core
{
    public enum QuestionType
    {
        MultipleChoice,
        ManualInput,
        OrderArrangement,
        Debugging
    }

    public class Question
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public Question(string text, QuestionType type, string correctAnswer, List<string> options = null, string explanation = "")
        {
            Text = text;
            Type = type;
            CorrectAnswer = correctAnswer;
            Options = options ?? new List<string>();
            Explanation = explanation;
        }

        public bool IsCorrect(string answer)
        {
            if (string.IsNullOrEmpty(answer)) return false;
            
            string cleanedAnswer = Normalize(answer);
            string cleanedCorrect = Normalize(CorrectAnswer);

            // 1. Direct match (covers Manual Input and exact Multiple Choice matches)
            if (cleanedAnswer == cleanedCorrect) return true;

            if (Type == QuestionType.MultipleChoice)
            {
                foreach (var option in Options)
                {
                    string cleanedOption = Normalize(option); // e.g. "a. loop"
                    
                    // 2. Check if user entered just the letter (e.g. "a")
                    if (cleanedAnswer.Length == 1 && cleanedOption.Length > 0 && cleanedAnswer[0] == cleanedOption[0])
                    {
                        // Double check that this option is actually the correct one
                        if (cleanedOption == cleanedCorrect) return true;
                    }

                    // 3. Check if user entered the text without the letter prefix (e.g. user types "loop" for "A. Loop")
                    // We look for the dot separator ". "
                    int dotIndex = cleanedOption.IndexOf('.');
                    if (dotIndex != -1 && dotIndex < cleanedOption.Length - 1)
                    {
                        string optionTextOnly = cleanedOption.Substring(dotIndex + 1).Trim();
                        if (cleanedAnswer == optionTextOnly && cleanedOption == cleanedCorrect)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private string Normalize(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Replace various dashes with standard hyphen
            string normalized = input.Replace('–', '-').Replace('—', '-');
            return normalized.Trim().ToLowerInvariant();
        }
    }
}
