using System.ComponentModel.DataAnnotations;

namespace Typist.Desktop.Models;

public class LessonProgress
{
    [Key]
    public int Id { get; set; }

    public int LessonId { get; set; }

    public int UserProfileId { get; set; }

    public double BestWpm { get; set; }

    public double BestAccuracy { get; set; }

    public int AttemptCount { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime LastAttemptAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Lesson? Lesson { get; set; }
    public UserProfile? UserProfile { get; set; }
}
