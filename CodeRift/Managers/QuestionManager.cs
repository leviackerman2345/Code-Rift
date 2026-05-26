using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using CodeRift.Entities;

namespace CodeRift.Managers
{
    /// <summary>
    /// QuestionManager: The brain that handles loading, storing, and 
    /// serving questions to the Battle Arena.
    /// </summary>
    public class QuestionManager
    {
        private static QuestionManager? _instance;
        public static QuestionManager Instance => _instance ??= new QuestionManager();

        private List<Question> _allQuestions = new List<Question>();
        private readonly Dictionary<int, List<Question>> _questionsByLevel = new Dictionary<int, List<Question>>();
        private Dictionary<int, int> _levelQuestionIndices = new Dictionary<int, int>();

        private QuestionManager()
        {
            LoadQuestions();
        }

        /// <summary>
        /// Loads the question bank from the JSON file.
        /// </summary>
        private void LoadQuestions()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Utils", "questions.json");
                
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var bank = JsonSerializer.Deserialize<QuestionBank>(json);
                    
                    if (bank != null)
                    {
                        _allQuestions = bank.Questions;
                        BuildLevelQuestionCache();
                        _levelQuestionIndices.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                // In a real game, you'd log this. For now, we'll show a warning.
                Console.WriteLine("Error loading questions: " + ex.Message);
            }
        }

        /// <summary>
        /// Fetches a random question filtered by the current level.
        /// </summary>
        /// <param name="level">The level of the current Battle Arena (1-5)</param>
        /// <returns>A Question object, or a fallback if none found.</returns>
        public Question GetRandomQuestion(int level)
        {
            if (_questionsByLevel.TryGetValue(level, out List<Question>? levelQuestions) && levelQuestions.Count > 0)
            {
                if (!_levelQuestionIndices.TryGetValue(level, out int index))
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
        /// Forces a reload of the questions (useful for debugging).
        /// </summary>
        public void Reload() => LoadQuestions();

        private void BuildLevelQuestionCache()
        {
            _questionsByLevel.Clear();

            foreach (Question question in _allQuestions)
            {
                if (!_questionsByLevel.TryGetValue(question.Level, out List<Question>? list))
                {
                    list = new List<Question>();
                    _questionsByLevel[question.Level] = list;
                }

                list.Add(question);
            }
        }
    }
}
