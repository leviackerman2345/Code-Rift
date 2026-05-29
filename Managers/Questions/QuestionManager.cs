using System;
using System.Collections.Generic;
using CodeRift.Entities;

namespace CodeRift.Managers
{
    /// <summary>
    /// QuestionManager: The brain that handles loading, storing, and
    /// serving questions to the Battle Arena.
    /// </summary>
    public class QuestionManager
    {
        private static QuestionManager _instance;
        public static QuestionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QuestionManager();
                }
                return _instance;
            }
        }

        private List<Question> _allQuestions = new List<Question>();
        private readonly Dictionary<int, List<Question>> _questionsByLevel = new Dictionary<int, List<Question>>();
        private Dictionary<int, int> _levelQuestionIndices = new Dictionary<int, int>();

        private QuestionManager()
        {
            LoadQuestions();
        }

        /// <summary>
        /// Loads the question bank from LanguageManager translation keys.
        /// </summary>
        private void LoadQuestions()
        {
            _allQuestions.Clear();
            var lang = LanguageManager.Instance;

            for (int level = 1; level <= 5; level++)
            {
                string levelTitle = lang.Get(string.Format("level_title_{0}", level));

                for (int q = 1; q <= 5; q++)
                {
                    string prefix = string.Format("q{0}_{1}", level, q);
                    string statementKey = prefix + "_statement";
                    string statement = lang.Get(statementKey);

                    // Skip if no translation found (key returns [key] when missing)
                    if (statement == string.Format("[{0}]", statementKey))
                        continue;

                    string answer = lang.Get(prefix + "_answer");
                    string hint = lang.Get(prefix + "_hint");

                    // Check for multiple choice options
                    var options = new List<string>();
                    for (int opt = 1; opt <= 4; opt++)
                    {
                        string optKey = string.Format("{0}_option_{1}", prefix, opt);
                        string optVal = lang.Get(optKey);
                        if (optVal == string.Format("[{0}]", optKey))
                            break;
                        options.Add(optVal);
                    }

                    QuestionType type = options.Count > 0 ? QuestionType.MultipleChoice : QuestionType.CodeInput;

                    _allQuestions.Add(new Question
                    {
                        Level = level,
                        LevelTitle = levelTitle,
                        Type = type,
                        ProblemStatement = statement,
                        Options = options.Count > 0 ? options : null,
                        CorrectAnswer = answer,
                        Hint = hint
                    });
                }
            }

            BuildLevelQuestionCache();
            _levelQuestionIndices.Clear();
        }

        /// <summary>
        /// Fetches a random question filtered by the current level.
        /// </summary>
        /// <param name="level">The level of the current Battle Arena (1-5)</param>
        /// <returns>A Question object, or a fallback if none found.</returns>
        public Question GetRandomQuestion(int level)
        {
            List<Question> levelQuestions;
            if (_questionsByLevel.TryGetValue(level, out levelQuestions) && levelQuestions.Count > 0)
            {
                int index;
                if (!_levelQuestionIndices.TryGetValue(level, out index))
                {
                    index = 0;
                }

                if (index >= levelQuestions.Count)
                {
                    index = 0;
                }

                _levelQuestionIndices[level] = index + 1;
                return levelQuestions[index];
            }

            // Fallback empty question to prevent crash if level has no data
            return new Question
            {
                ProblemStatement = "SYS_ERR: Question data missing for this level.",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "RETRY", "CANCEL", "DEBUG", "EXIT" }
            };
        }

        /// <summary>
        /// Forces a reload of the questions (useful after language switch).
        /// </summary>
        public void Reload() { LoadQuestions(); }

        private void BuildLevelQuestionCache()
        {
            _questionsByLevel.Clear();

            foreach (Question question in _allQuestions)
            {
                List<Question> list;
                if (!_questionsByLevel.TryGetValue(question.Level, out list))
                {
                    list = new List<Question>();
                    _questionsByLevel[question.Level] = list;
                }

                list.Add(question);
            }
        }
    }
}
