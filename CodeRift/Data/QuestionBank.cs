using System.Collections.Generic;
using CodeRift.Models;

namespace CodeRift.Data
{
    public static class QuestionBank
    {
        public static List<Question> GetAll()
        {
            return new List<Question>
            {
                // Level 1: Loops
                new Question {
                    Id = 1, Level = 1, Type = "Manual",
                    Text = "What keyword starts a loop that checks the condition after executing the body?",
                    CorrectAnswer = "do",
                    Explanation = "A do-while loop executes at least once.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 2, Level = 1, Type = "Manual",
                    Text = "Fill in the blank for a 10-iteration loop: for (int i = 0; i < ___; i++)",
                    CorrectAnswer = "10",
                    Explanation = "The condition 'i < 10' runs the loop 10 times (0-9).",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 3, Level = 1, Type = "Manual",
                    Text = "What keyword is used to skip the rest of the current loop iteration?",
                    CorrectAnswer = "continue",
                    Explanation = "continue jumps to the next iteration of the loop.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 4, Level = 1, Type = "Manual",
                    Text = "What keyword is used to exit a loop immediately?",
                    CorrectAnswer = "break",
                    Explanation = "break terminates the nearest enclosing loop.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 5, Level = 1, Type = "Manual",
                    Text = "Which loop type is ideal for iterating through every item in a list?",
                    CorrectAnswer = "foreach",
                    Explanation = "foreach simplifies iterating over collections.",
                    Damage = 20, EnemyDamage = 20
                },

                // Level 2: Methods
                new Question {
                    Id = 6, Level = 2, Type = "Manual",
                    Text = "What keyword indicates a method returns no value?",
                    CorrectAnswer = "void",
                    Explanation = "void is the return type for methods that don't return data.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 7, Level = 2, Type = "Manual",
                    Text = "What keyword is used to send a value back from a method?",
                    CorrectAnswer = "return",
                    Explanation = "return exits the method and provides a result.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 8, Level = 2, Type = "Manual",
                    Text = "Are variables in a method signature called 'parameters' or 'arguments'?",
                    CorrectAnswer = "parameters",
                    Explanation = "Parameters are defined in the signature; arguments are passed values.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 9, Level = 2, Type = "Manual",
                    Text = "What keyword allows a method to be called without creating an instance of a class?",
                    CorrectAnswer = "static",
                    Explanation = "Static methods belong to the class itself.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 10, Level = 2, Type = "Manual",
                    Text = "A method that calls itself is known as what? (one word)",
                    CorrectAnswer = "recursion",
                    Explanation = "Recursion is the process of a method calling itself.",
                    Damage = 20, EnemyDamage = 20
                },

                // Level 3: Strings
                new Question {
                    Id = 11, Level = 3, Type = "Manual",
                    Text = "Which string property returns the character count?",
                    CorrectAnswer = "Length",
                    Explanation = "Length returns the number of characters.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 12, Level = 3, Type = "Manual",
                    Text = "What method converts all characters in a string to uppercase?",
                    CorrectAnswer = "ToUpper",
                    Explanation = "ToUpper() creates an uppercase copy of the string.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 13, Level = 3, Type = "Manual",
                    Text = "Which method checks if one string exists inside another?",
                    CorrectAnswer = "Contains",
                    Explanation = "Contains returns true if the substring is found.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 14, Level = 3, Type = "Manual",
                    Text = "What character is used to access an individual character in a string by index? (e.g. s[0])",
                    CorrectAnswer = "[",
                    Explanation = "The indexer [ ] allows access to specific characters.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 15, Level = 3, Type = "Manual",
                    Text = "What method is used to remove leading and trailing whitespace?",
                    CorrectAnswer = "Trim",
                    Explanation = "Trim() cleans up whitespace from both ends.",
                    Damage = 20, EnemyDamage = 20
                },

                // Level 4: Arrays
                new Question {
                    Id = 16, Level = 4, Type = "Manual",
                    Text = "What is the index of the first element in any C# array?",
                    CorrectAnswer = "0",
                    Explanation = "Arrays are zero-indexed.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 17, Level = 4, Type = "Manual",
                    Text = "What array property returns the total number of elements?",
                    CorrectAnswer = "Length",
                    Explanation = "Length is used for arrays (Count is for Lists).",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 18, Level = 4, Type = "Manual",
                    Text = "How do you access the 3rd element of an array 'arr'?",
                    CorrectAnswer = "arr[2]",
                    Explanation = "Index 2 is the 3rd element.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 19, Level = 4, Type = "Manual",
                    Text = "What keyword is used to initialize a new array instance?",
                    CorrectAnswer = "new",
                    Explanation = "new int[5] allocates memory for the array.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 20, Level = 4, Type = "Manual",
                    Text = "Can the size of a C# array be changed after it is created? (Yes/No)",
                    CorrectAnswer = "No",
                    Explanation = "Arrays are fixed-size. Use List for dynamic sizing.",
                    Damage = 20, EnemyDamage = 20
                },

                // Level 5: Boss
                new Question {
                    Id = 21, Level = 5, Type = "Manual",
                    Text = "What exception occurs when accessing an array index that doesn't exist?",
                    CorrectAnswer = "IndexOutOfRangeException",
                    Explanation = "Accessing out of bounds throws this exception.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 22, Level = 5, Type = "Manual",
                    Text = "What is the result of '10' + 5 in C#? (e.g. \"105\")",
                    CorrectAnswer = "105",
                    Explanation = "String + int results in string concatenation.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 23, Level = 5, Type = "Manual",
                    Text = "What type of loop is 'foreach' internally based on? (while/for)",
                    CorrectAnswer = "while",
                    Explanation = "foreach uses an enumerator in a while loop.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 24, Level = 5, Type = "Manual",
                    Text = "What is the default value of an uninitialized int variable in a class?",
                    CorrectAnswer = "0",
                    Explanation = "Numeric types default to zero.",
                    Damage = 20, EnemyDamage = 20
                },
                new Question {
                    Id = 25, Level = 5, Type = "Manual",
                    Text = "Which keyword is used to refer to the current instance of a class?",
                    CorrectAnswer = "this",
                    Explanation = "the 'this' keyword refers to the current object.",
                    Damage = 20, EnemyDamage = 20
                }
            };
        }
    }
}
