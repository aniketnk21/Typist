using System.ComponentModel.DataAnnotations;

namespace Typist.Desktop.Models;

public enum Difficulty
{
    Beginner,
    Intermediate,
    Advanced
}

public enum LessonCategory
{
    HomeRow,
    TopRow,
    BottomRow,
    Numbers,
    CommonWords,
    Sentences,
    Paragraphs,
    CodeSnippets
}

public class Lesson
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public Difficulty Difficulty { get; set; }

    public LessonCategory Category { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int OrderIndex { get; set; }

    // Navigation
    public List<LessonProgress> Progresses { get; set; } = new();
}
