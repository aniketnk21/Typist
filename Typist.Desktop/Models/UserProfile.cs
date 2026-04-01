using System.ComponentModel.DataAnnotations;

namespace Typist.Desktop.Models;

public class UserProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = "Default";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TotalTestsTaken { get; set; }

    public double BestWpm { get; set; }

    public double AverageWpm { get; set; }

    public double AverageAccuracy { get; set; }

    // Navigation
    public List<TestResult> TestResults { get; set; } = new();
    public List<LessonProgress> LessonProgresses { get; set; } = new();
}
