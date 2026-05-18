using System.Collections.Generic;
using CodeRift.Core;

namespace CodeRift.Managers
{
    public static class QuestionManager
    {
        private static readonly Dictionary<string, Dictionary<int, List<Question>>> LocalizedLevelQuestions = new Dictionary<string, Dictionary<int, List<Question>>>();

        static QuestionManager()
        {
            InitializeQuestions();
        }

        private static void InitializeQuestions()
        {
            // ENGLISH QUESTIONS
            var enQuestions = new Dictionary<int, List<Question>>();
            LocalizedLevelQuestions["en"] = enQuestions;

            enQuestions[1] = new List<Question>
            {
                new Question("A _______________is a basic programming construct that allows repeated execution of a fragment of source code.", 
                    QuestionType.MultipleChoice, "A. Loop", new List<string> { "A. Loop", "B. Termination", "C. Increment", "D. Do while" }),
                new Question("_____________ is any expression that returns a Boolean result – true or false.", 
                    QuestionType.MultipleChoice, "C. Condition", new List<string> { "A. Loop", "B. Termination", "C. Condition", "D. Do while" }),
                new Question("In for (int i = 0; i < 10; i++), what is the increment part?", 
                    QuestionType.ManualInput, "i++"),
                new Question("In for (int i = 0; i < 10; i++), what is the initialization part?", 
                    QuestionType.ManualInput, "int i = 0"),
                new Question("The ______________are programming constructs consisting of several loops located into each other.", 
                    QuestionType.MultipleChoice, "B. Nested loops", new List<string> { "A. Do while", "B. Nested loops", "C. Foreach", "D. While" })
            };

            enQuestions[2] = new List<Question>
            {
                new Question("______________ is the building block of object-oriented programming. It combines related code together and makes program easier.", 
                    QuestionType.MultipleChoice, "B. Method", new List<string> { "A. User-Defined Method", "B. Method", "C. Body", "D. Return Type" }),
                new Question("Arrange the correct order of a method header:\n1. Return Type\n2. Parameter List\n3. Method Name\n4. Access Specifier", 
                    QuestionType.OrderArrangement, "4 – 1 – 3 – 2"),
                new Question("A method may return a value. __________", 
                    QuestionType.ManualInput, "Return Type"),
                new Question("What keyword do you use as the return type if the method does not return any value?", 
                    QuestionType.ManualInput, "Void"),
                new Question("This contains the set of instructions needed to complete the required activity.", 
                    QuestionType.MultipleChoice, "C. Method body", new List<string> { "A. Method header", "B. Access Specifier", "C. Method body", "D. Return type" })
            };

            enQuestions[3] = new List<Question>
            {
                new Question("What is a string in C#?", 
                    QuestionType.MultipleChoice, "B. A data type that can store sequence of characters", new List<string> { "A. A numeric data type", "B. A data type that can store sequence of characters", "C. A special loop", "D. An array of integers" }),
                new Question("It Removes all the characters in the current instance, beginning at a specified position and continuing through the last position, and returns the string.", 
                    QuestionType.MultipleChoice, "C. Remove()", new List<string> { "A. Concat()", "B. Clear()", "C. Remove()", "D. Delete()" }),
                new Question("What index number is the first character of a string?", 
                    QuestionType.ManualInput, "0"),
                new Question("Fix the error in this C#:\nstring name = BADing;", 
                    QuestionType.Debugging, "string name = \"BADing\";"),
                new Question("What is the output?\nstring txt = \"CodeRift\";\nConsole.WriteLine(txt.Substring(4));", 
                    QuestionType.Debugging, "Rift")
            };

            enQuestions[4] = new List<Question>
            {
                new Question("What is true about arrays in C#?", 
                    QuestionType.MultipleChoice, "B. Fixed size, stores same data type elements", new List<string> { "A. Can store different data types together", "B. Fixed size, stores same data type elements", "C. Always starts at index 1", "D. Grows automatically" }),
                new Question("int[] numbers = new int[5]; How do you access the third element?", 
                    QuestionType.MultipleChoice, "B. numbers[2]", new List<string> { "A. numbers[3]", "B. numbers[2]", "C. numbers(2)", "D. numbers{3}" }),
                new Question("Arrange correct steps to create and fill an array in C#:\n1. Assign values: numbers[0] = 10;\n2. Declare type and name: int[] numbers;\n3. Allocate memory: numbers = new int[5];", 
                    QuestionType.OrderArrangement, "2 - 3 - 1"),
                new Question("string[] items = new string[10];\nHow many elements can this array store?", 
                    QuestionType.ManualInput, "10"),
                new Question("What exception error occurs if you try to access an index outside the array size?", 
                    QuestionType.ManualInput, "IndexOutOfRangeException")
            };

            enQuestions[5] = new List<Question>
            {
                new Question("What is the output?\nstring s1 = \"CODE\";\nstring s2 = String.Concat(s1, \"monster\");\nConsole.WriteLine(\"{0}\",s2);", 
                    QuestionType.Debugging, "CODEmonster"),
                new Question("What is the output?\nnames.Enqueue(\"NICK\");\nnames.Enqueue(\"Bob\");\nnames.Enqueue(\"Charlie\");\nConsole.WriteLine(\"First person: \" + names.Peek());\nnames.Dequeue();\nforeach (string name in names) { Console.WriteLine(name); }", 
                    QuestionType.Debugging, "First person: NICK\nAfter dequeue:\nBob\nCharlie"),
                new Question("Find and fix the error:\nint[] scores = { 80, 90, 95 };\nfor (int i = 0; i <= scores.Length; i++)\nConsole.WriteLine(scores[i]);", 
                    QuestionType.Debugging, "i < scores.Length", null, "Change i <= scores.Length to i < scores.Length"),
                new Question("What is the output?\nstring word = \"C#Game\"; int len = word.Length;\n// result = word.Length * 3 (assumed logic)\nConsole.WriteLine(result);", 
                    QuestionType.Debugging, "18"),
                new Question("What is the output?\nnumbers.Push(100);\nnumbers.Push(99);\nnumbers.Push(59);\nConsole.WriteLine(\"Top number: \" + numbers.Peek());\nnumbers.Pop();\nforeach (int num in numbers) { Console.WriteLine(num); }", 
                    QuestionType.Debugging, "Top number: 59\nAfter pop:\n99\n100")
            };

            // TAGALOG QUESTIONS (PH)
            var phQuestions = new Dictionary<int, List<Question>>();
            LocalizedLevelQuestions["ph"] = phQuestions;

            phQuestions[1] = new List<Question>
            {
                new Question("Ang _______________ ay isang pangunahing programming construct na nagbibigay-daan sa paulit-ulit na pag-execute ng isang bahagi ng source code.", 
                    QuestionType.MultipleChoice, "A. Loop", new List<string> { "A. Loop", "B. Termination", "C. Increment", "D. Do while" }),
                new Question("Ang _____________ ay anumang expression na nagbabalik ng Boolean na resulta – true o false.", 
                    QuestionType.MultipleChoice, "C. Condition", new List<string> { "A. Loop", "B. Termination", "C. Condition", "D. Do while" }),
                new Question("Sa for (int i = 0; i < 10; i++), ano ang bahagi ng increment?", 
                    QuestionType.ManualInput, "i++"),
                new Question("Sa for (int i = 0; i < 10; i++), ano ang bahagi ng initialization?", 
                    QuestionType.ManualInput, "int i = 0"),
                new Question("Ang ______________ ay mga programming construct na binubuo ng ilang loops na nasa loob ng isa't isa.", 
                    QuestionType.MultipleChoice, "B. Nested loops", new List<string> { "A. Do while", "B. Nested loops", "C. Foreach", "D. While" })
            };

            phQuestions[2] = new List<Question>
            {
                new Question("Ang ______________ ay ang building block ng object-oriented programming. Pinagsasama nito ang magkakaugnay na code at ginagawang mas madali ang programa.", 
                    QuestionType.MultipleChoice, "B. Method", new List<string> { "A. User-Defined Method", "B. Method", "C. Body", "D. Return Type" }),
                new Question("Ayusin ang tamang pagkakasunod-sunod ng isang method header:\n1. Return Type\n2. Parameter List\n3. Method Name\n4. Access Specifier", 
                    QuestionType.OrderArrangement, "4 – 1 – 3 – 2"),
                new Question("Ang isang method ay maaaring magbalik ng halaga. __________", 
                    QuestionType.ManualInput, "Return Type"),
                new Question("Anong keyword ang ginagamit mo bilang return type kung ang method ay hindi nagbabalik ng anumang halaga?", 
                    QuestionType.ManualInput, "Void"),
                new Question("Ito ay naglalaman ng hanay ng mga tagubilin na kinakailangan upang makumpleto ang kinakailangang aktibidad.", 
                    QuestionType.MultipleChoice, "C. Method body", new List<string> { "A. Method header", "B. Access Specifier", "C. Method body", "D. Return type" })
            };

            phQuestions[3] = new List<Question>
            {
                new Question("Ano ang string sa C#?", 
                    QuestionType.MultipleChoice, "B. Isang data type na maaaring mag-imbak ng pagkakasunod-sunod ng mga character", new List<string> { "A. Isang numeric data type", "B. Isang data type na maaaring mag-imbak ng pagkakasunod-sunod ng mga character", "C. Isang espesyal na loop", "D. Isang array ng mga integer" }),
                new Question("Inaalis nito ang lahat ng mga character sa kasalukuyang instance, simula sa isang tinukoy na posisyon at nagpapatuloy hanggang sa huling posisyon, at ibinabalik ang string.", 
                    QuestionType.MultipleChoice, "C. Remove()", new List<string> { "A. Concat()", "B. Clear()", "C. Remove()", "D. Delete()" }),
                new Question("Anong index number ang unang character ng isang string?", 
                    QuestionType.ManualInput, "0"),
                new Question("Ayusin ang error sa C# na ito:\nstring name = BADing;", 
                    QuestionType.Debugging, "string name = \"BADing\";"),
                new Question("Ano ang output?\nstring txt = \"CodeRift\";\nConsole.WriteLine(txt.Substring(4));", 
                    QuestionType.Debugging, "Rift")
            };

            phQuestions[4] = new List<Question>
            {
                new Question("Ano ang totoo tungkol sa mga array sa C#?", 
                    QuestionType.MultipleChoice, "B. Fixed size, nag-iimbak ng parehong data type na mga elemento", new List<string> { "A. Maaaring mag-imbak ng iba't ibang data type nang magkasama", "B. Fixed size, nag-iimbak ng parehong data type na mga elemento", "C. Palaging nagsisimula sa index 1", "D. Awtomatikong lumalaki" }),
                new Question("int[] numbers = new int[5]; Paano mo maa-access ang ikatlong elemento?", 
                    QuestionType.MultipleChoice, "B. numbers[2]", new List<string> { "A. numbers[3]", "B. numbers[2]", "C. numbers(2)", "D. numbers{3}" }),
                new Question("Ayusin ang tamang mga hakbang upang lumikha at punan ang isang array sa C#:\n1. Magtalaga ng mga halaga: numbers[0] = 10;\n2. I-declare ang type at pangalan: int[] numbers;\n3. Maglaan ng memorya: numbers = new int[5];", 
                    QuestionType.OrderArrangement, "2 - 3 - 1"),
                new Question("string[] items = new string[10];\nIlang elemento ang maaaring i-imbak ng array na ito?", 
                    QuestionType.ManualInput, "10"),
                new Question("Anong exception error ang nangyayari kung susubukan mong i-access ang isang index sa labas ng laki ng array?", 
                    QuestionType.ManualInput, "IndexOutOfRangeException")
            };

            phQuestions[5] = new List<Question>
            {
                new Question("Ano ang output?\nstring s1 = \"CODE\";\nstring s2 = String.Concat(s1, \"monster\");\nConsole.WriteLine(\"{0}\",s2);", 
                    QuestionType.Debugging, "CODEmonster"),
                new Question("Ano ang output?\nnames.Enqueue(\"NICK\");\nnames.Enqueue(\"Bob\");\nnames.Enqueue(\"Charlie\");\nConsole.WriteLine(\"First person: \" + names.Peek());\nnames.Dequeue();\nforeach (string name in names) { Console.WriteLine(name); }", 
                    QuestionType.Debugging, "First person: NICK\nAfter dequeue:\nBob\nCharlie"),
                new Question("Hanapin at ayusin ang error:\nint[] scores = { 80, 90, 95 };\nfor (int i = 0; i <= scores.Length; i++)\nConsole.WriteLine(scores[i]);", 
                    QuestionType.Debugging, "i < scores.Length", null, "Palitan ang i <= scores.Length ng i < scores.Length"),
                new Question("Ano ang output?\nstring word = \"C#Game\"; int len = word.Length;\n// result = word.Length * 3 (assumed logic)\nConsole.WriteLine(result);", 
                    QuestionType.Debugging, "18"),
                new Question("Ano ang output?\nnumbers.Push(100);\nnumbers.Push(99);\nnumbers.Push(59);\nConsole.WriteLine(\"Top number: \" + numbers.Peek());\nnumbers.Pop();\nforeach (int num in numbers) { Console.WriteLine(num); }", 
                    QuestionType.Debugging, "Top number: 59\nAfter pop:\n99\n100")
            };
        }

        public static List<Question> GetQuestionsForLevel(int level, string lang = "en")
        {
            if (LocalizedLevelQuestions.TryGetValue(lang, out var levelDict))
            {
                if (levelDict.TryGetValue(level, out var questions))
                {
                    return questions;
                }
            }
            // Fallback to English if not found
            if (lang != "en" && LocalizedLevelQuestions.TryGetValue("en", out var enDict))
            {
                if (enDict.TryGetValue(level, out var enQuestions))
                {
                    return enQuestions;
                }
            }
            return new List<Question>();
        }
    }
}
