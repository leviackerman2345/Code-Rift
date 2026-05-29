using System;

namespace CodeRift.Core
{
    public enum QuestionSkipCommandType
    {
        None,
        SkipCurrentQuestion,
        SkipAllQuestions
    }

    public static class QuestionSkipCommand
    {
        public const string SkipCurrentQuestionText = "///";
        public const string SkipAllQuestionsText = "/////";

        public static QuestionSkipCommandType Parse(string input)
        {
            string command = (input ?? string.Empty).Trim();

            if (string.Equals(command, SkipAllQuestionsText, StringComparison.Ordinal))
            {
                return QuestionSkipCommandType.SkipAllQuestions;
            }

            if (string.Equals(command, SkipCurrentQuestionText, StringComparison.Ordinal))
            {
                return QuestionSkipCommandType.SkipCurrentQuestion;
            }

            return QuestionSkipCommandType.None;
        }
    }
}
