using Typist.Desktop.Models;

namespace Typist.Desktop.Data;

public static class SeedData
{
    public static List<Lesson> GetLessons()
    {
        return new List<Lesson>
        {
            // === BEGINNER: Home Row ===
            new() { Id = 1, Title = "Home Row Basics", Difficulty = Difficulty.Beginner, Category = LessonCategory.HomeRow, OrderIndex = 1,
                Description = "Learn the home row keys: A S D F J K L ;",
                Content = "asdf jkl; asdf jkl; asdf jkl; fj dk sl a; fj dk sl a; asd jkl fds ;lk asdf jkl; fdsa ;lkj" },

            new() { Id = 2, Title = "Home Row Words", Difficulty = Difficulty.Beginner, Category = LessonCategory.HomeRow, OrderIndex = 2,
                Description = "Type simple words using home row keys",
                Content = "sad lad dad all fall lass salad flask add ask ads glad glad sad dad lad flask salad fall all lass add" },

            new() { Id = 3, Title = "Home Row Sentences", Difficulty = Difficulty.Beginner, Category = LessonCategory.HomeRow, OrderIndex = 3,
                Description = "Practice home row with complete sentences",
                Content = "a lad had a salad. a lass had a flask. dad falls. all lads add. a sad lass asks dad. all fall." },

            // === BEGINNER: Top Row ===
            new() { Id = 4, Title = "Top Row Introduction", Difficulty = Difficulty.Beginner, Category = LessonCategory.TopRow, OrderIndex = 4,
                Description = "Learn the top row keys: Q W E R T Y U I O P",
                Content = "qwer tyui op qwer tyui op re ti ew uo qw op er ty ui we rt yo iu qw op re ti we uo qw po" },

            new() { Id = 5, Title = "Top Row Words", Difficulty = Difficulty.Beginner, Category = LessonCategory.TopRow, OrderIndex = 5,
                Description = "Common words using top and home row keys",
                Content = "the that with were they your what this will work just like people would first quite write type rope trip" },

            new() { Id = 6, Title = "Top Row Practice", Difficulty = Difficulty.Beginner, Category = LessonCategory.TopRow, OrderIndex = 6,
                Description = "Extended practice with top and home row",
                Content = "power tower flower quest require equip quiet toward report sort port sport effort supporter tower fire retire wire riot" },

            // === BEGINNER: Bottom Row ===
            new() { Id = 7, Title = "Bottom Row Introduction", Difficulty = Difficulty.Beginner, Category = LessonCategory.BottomRow, OrderIndex = 7,
                Description = "Learn the bottom row keys: Z X C V B N M",
                Content = "zxcv bnm zxcv bnm zx cv bn mx zc vb nm zx cv bn mx cb vn mz xc vb nm zx cv bn" },

            new() { Id = 8, Title = "Bottom Row Words", Difficulty = Difficulty.Beginner, Category = LessonCategory.BottomRow, OrderIndex = 8,
                Description = "Words using all three rows",
                Content = "have been come back much make can may next never between became because number name over move even most know" },

            // === INTERMEDIATE: Common Words ===
            new() { Id = 9, Title = "100 Most Common Words", Difficulty = Difficulty.Intermediate, Category = LessonCategory.CommonWords, OrderIndex = 9,
                Description = "Practice the 100 most frequently used English words",
                Content = "the be to of and a in that have I it for not on with he as you do at this but his by from" },

            new() { Id = 10, Title = "Speed Building Words", Difficulty = Difficulty.Intermediate, Category = LessonCategory.CommonWords, OrderIndex = 10,
                Description = "Build speed with common word combinations",
                Content = "about would there their what which when could other than more these some time very your has been would make like just know take people" },

            new() { Id = 11, Title = "Advanced Common Words", Difficulty = Difficulty.Intermediate, Category = LessonCategory.CommonWords, OrderIndex = 11,
                Description = "Less common but important words",
                Content = "government important development environment information international different university something national another possible experience education community business continue technology" },

            // === INTERMEDIATE: Numbers ===
            new() { Id = 12, Title = "Number Row Basics", Difficulty = Difficulty.Intermediate, Category = LessonCategory.Numbers, OrderIndex = 12,
                Description = "Practice typing numbers 1-0",
                Content = "123 456 789 012 345 678 901 234 567 890 111 222 333 444 555 666 777 888 999 000 1234 5678 9012 3456" },

            new() { Id = 13, Title = "Numbers in Context", Difficulty = Difficulty.Intermediate, Category = LessonCategory.Numbers, OrderIndex = 13,
                Description = "Numbers mixed with words",
                Content = "Flight 247 departs at 3:45 PM from gate 12. The package weighs 18.5 kg and costs $299.99. Call 555-0123 for information." },

            // === INTERMEDIATE: Sentences ===
            new() { Id = 14, Title = "Short Sentences", Difficulty = Difficulty.Intermediate, Category = LessonCategory.Sentences, OrderIndex = 14,
                Description = "Practice with short, varied sentences",
                Content = "The quick brown fox jumps over the lazy dog. Pack my box with five dozen liquor jugs. How vexingly quick daft zebras jump!" },

            new() { Id = 15, Title = "Medium Sentences", Difficulty = Difficulty.Intermediate, Category = LessonCategory.Sentences, OrderIndex = 15,
                Description = "Longer sentences for building endurance",
                Content = "Success is not final, failure is not fatal: it is the courage to continue that counts. The only way to do great work is to love what you do." },

            new() { Id = 16, Title = "Punctuation Practice", Difficulty = Difficulty.Intermediate, Category = LessonCategory.Sentences, OrderIndex = 16,
                Description = "Sentences with varied punctuation",
                Content = "Hello, world! How are you? I'm fine, thanks. She said, \"Let's go!\" The cost is $49.99 (plus tax). Email: user@example.com" },

            // === ADVANCED: Paragraphs ===
            new() { Id = 17, Title = "Paragraph Endurance 1", Difficulty = Difficulty.Advanced, Category = LessonCategory.Paragraphs, OrderIndex = 17,
                Description = "Build typing endurance with full paragraphs",
                Content = "Programming is the art of telling a computer what to do. It requires patience, logic, and creativity. A good programmer writes code that humans can understand. Clean code is not just about making things work; it is about making things right. Every line of code should have a purpose, and every function should do one thing well." },

            new() { Id = 18, Title = "Paragraph Endurance 2", Difficulty = Difficulty.Advanced, Category = LessonCategory.Paragraphs, OrderIndex = 18,
                Description = "Advanced paragraph with varied vocabulary",
                Content = "The evolution of technology has fundamentally transformed how we communicate, work, and learn. From the invention of the printing press to the rise of the internet, each breakthrough has expanded human potential. Today, artificial intelligence represents the next frontier, promising to revolutionize industries and reshape society in ways we are only beginning to understand." },

            new() { Id = 19, Title = "Paragraph Endurance 3", Difficulty = Difficulty.Advanced, Category = LessonCategory.Paragraphs, OrderIndex = 19,
                Description = "Complex vocabulary and sentence structure",
                Content = "Effective communication is an essential skill in both personal and professional contexts. It involves not only the clear articulation of ideas but also active listening and empathy. Whether writing an email, delivering a presentation, or having a difficult conversation, the ability to convey your message with clarity and respect can make the difference between success and misunderstanding." },

            // === ADVANCED: Code Snippets ===
            new() { Id = 20, Title = "Code: Variables & Functions", Difficulty = Difficulty.Advanced, Category = LessonCategory.CodeSnippets, OrderIndex = 20,
                Description = "Practice typing code syntax",
                Content = "int count = 0; string name = \"Typist\"; var items = new List<string>(); public void PrintMessage(string msg) { Console.WriteLine(msg); }" },

            new() { Id = 21, Title = "Code: Loops & Conditions", Difficulty = Difficulty.Advanced, Category = LessonCategory.CodeSnippets, OrderIndex = 21,
                Description = "Loops, conditionals, and brackets",
                Content = "for (int i = 0; i < 10; i++) { if (i % 2 == 0) { Console.Write(i); } } while (count > 0) { count--; } switch (key) { case 'a': break; }" },

            new() { Id = 22, Title = "Code: Classes & Objects", Difficulty = Difficulty.Advanced, Category = LessonCategory.CodeSnippets, OrderIndex = 22,
                Description = "Object-oriented programming syntax",
                Content = "public class Player { public string Name { get; set; } public int Score { get; set; } public Player(string name) { Name = name; Score = 0; } }" },
        };
    }

    public static List<string> GetTestTexts()
    {
        return new List<string>
        {
            "The quick brown fox jumps over the lazy dog. Pack my box with five dozen liquor jugs. How vexingly quick daft zebras jump! The five boxing wizards jump quickly at dawn.",

            "Success is not final, failure is not fatal: it is the courage to continue that counts. The only way to do great work is to love what you do. If you can dream it, you can do it.",

            "Technology is best when it brings people together. Innovation distinguishes between a leader and a follower. The computer was born to solve problems that did not exist before.",

            "In the middle of every difficulty lies opportunity. Life is what happens when you are busy making other plans. The purpose of our lives is to be happy and to help others find happiness.",

            "Programming is the closest thing we have to a superpower. First, solve the problem. Then, write the code. Code is like humor. When you have to explain it, it is bad.",

            "about after again air all along also an and another any are around as at away back be because been before began being below between both but by came can come could day did different do does each end even few find first for from get give go going good great had has have he help her here him his home house how I if in into is it its just know large last left let life like line little long look made make man many may me men more most much must my name never new next no not now number of off old on once one only or other our out over own part people place point program right",

            "She sells seashells by the seashore. The shells she sells are seashells, I am sure. Peter Piper picked a peck of pickled peppers. A peck of pickled peppers Peter Piper picked. If Peter Piper picked a peck of pickled peppers, where is the peck of pickled peppers that Peter Piper picked?",

            "The development of full artificial intelligence could spell the end of the human race. It would take off on its own and redesign itself at an ever-increasing rate. Humans, who are limited by slow biological evolution, could not compete and would be superseded.",
        };
    }
}
