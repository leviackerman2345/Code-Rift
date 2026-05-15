using System;
using System.Collections.Generic;
using System.Linq;
using CodeRift.Data;
using CodeRift.Models;

namespace CodeRift.Managers
{
    public class LevelManager
    {
        private List<Question> _currentLevelQuestions = new();
        private int _currentIndex;

        public void LoadLevel(int level)
        {
            _currentLevelQuestions = QuestionBank.GetAll()
                .Where(q => q.Level == level)
                .OrderBy(q => Guid.NewGuid()) 
                .Take(5) 
                .ToList();
            _currentIndex = 0;
        }

        public Question? GetNextQuestion()
        {
            if (HasMoreQuestions())
            {
                return _currentLevelQuestions[_currentIndex++];
            }
            return null;
        }

        public bool HasMoreQuestions() => _currentIndex < _currentLevelQuestions.Count;
        
        public int TotalQuestions => _currentLevelQuestions.Count;
        public int CurrentQuestionNumber => _currentIndex;

        public Enemy? GetEnemyForLevel(int level)
        {
            return level switch
            {
                1 => new Enemy { Name = "LoopBug", MaxHP = 80, CurrentHP = 80, Description = "A creature that endlessly repeats itself" },
                2 => new Enemy { Name = "VoidCrawler", MaxHP = 100, CurrentHP = 100, Description = "A shapeless Bug that mimics methods" },
                3 => new Enemy { Name = "StringCorruptor", MaxHP = 120, CurrentHP = 120, Description = "A Bug that twists and breaks text" },
                4 => new Enemy { Name = "ArrayWorm", MaxHP = 150, CurrentHP = 150, Description = "A multi-segmented Bug from broken arrays" },
                5 => new Enemy { Name = "The Null King", MaxHP = 200, CurrentHP = 200, Description = "The supreme Bug, origin of all corruption" },
                _ => null
            };
        }
    }
}
