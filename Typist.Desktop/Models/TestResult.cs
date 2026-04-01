using System.ComponentModel.DataAnnotations;

namespace Typist.Desktop.Models;

public enum TestType
{
    Quick,       // No time limit
    Timed1Min,   // 1 minute
    Timed2Min,   // 2 minutes
    Timed5Min,   // 5 minutes
    Lesson       // From lesson mode
}

public class TestResult
{
    [Key]
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public double Wpm { get; set; }

    public double RawWpm { get; set; }

    public double Accuracy { get; set; }

    public int DurationSeconds { get; set; }

    public int TotalCharacters { get; set; }

    public int CorrectCharacters { get; set; }

    public int IncorrectCharacters { get; set; }

    public int TotalWords { get; set; }

    public int CorrectWords { get; set; }

    public TestType TestType { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public UserProfile? UserProfile { get; set; }
}
