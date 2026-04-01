using Microsoft.EntityFrameworkCore;
using Typist.Desktop.Data;
using Typist.Desktop.Models;

namespace Typist.Desktop.Services;

public class DatabaseService
{
    private readonly TypistDbContext _context;

    public DatabaseService(TypistDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        // Seed lessons if empty
        if (!await _context.Lessons.AnyAsync())
        {
            var lessons = SeedData.GetLessons();
            _context.Lessons.AddRange(lessons);
            await _context.SaveChangesAsync();
        }

        // Create default user if not exists
        if (!await _context.UserProfiles.AnyAsync())
        {
            _context.UserProfiles.Add(new UserProfile
            {
                Username = "Default",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }

    // === User Profile ===
    public async Task<UserProfile> GetDefaultUserAsync()
    {
        return await _context.UserProfiles.FirstAsync();
    }

    public async Task UpdateUserStatsAsync(int userId)
    {
        var user = await _context.UserProfiles.FindAsync(userId);
        if (user == null) return;

        var results = await _context.TestResults
            .Where(r => r.UserProfileId == userId)
            .ToListAsync();

        user.TotalTestsTaken = results.Count;
        user.BestWpm = results.Count > 0 ? results.Max(r => r.Wpm) : 0;
        user.AverageWpm = results.Count > 0 ? Math.Round(results.Average(r => r.Wpm), 1) : 0;
        user.AverageAccuracy = results.Count > 0 ? Math.Round(results.Average(r => r.Accuracy), 1) : 0;

        await _context.SaveChangesAsync();
    }

    // === Lessons ===
    public async Task<List<Lesson>> GetLessonsAsync(Difficulty? difficulty = null, LessonCategory? category = null)
    {
        var query = _context.Lessons.AsQueryable();

        if (difficulty.HasValue)
            query = query.Where(l => l.Difficulty == difficulty.Value);

        if (category.HasValue)
            query = query.Where(l => l.Category == category.Value);

        return await query.OrderBy(l => l.OrderIndex).ToListAsync();
    }

    public async Task<Lesson?> GetLessonAsync(int lessonId)
    {
        return await _context.Lessons.FindAsync(lessonId);
    }

    // === Test Results ===
    public async Task SaveTestResultAsync(TestResult result)
    {
        _context.TestResults.Add(result);
        await _context.SaveChangesAsync();
        await UpdateUserStatsAsync(result.UserProfileId);
    }

    public async Task<List<TestResult>> GetTestResultsAsync(int userId, int? limit = null)
    {
        var query = _context.TestResults
            .Where(r => r.UserProfileId == userId)
            .OrderByDescending(r => r.CompletedAt);

        if (limit.HasValue)
            return await query.Take(limit.Value).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<List<TestResult>> GetTestResultsInRangeAsync(int userId, DateTime from, DateTime to)
    {
        return await _context.TestResults
            .Where(r => r.UserProfileId == userId && r.CompletedAt >= from && r.CompletedAt <= to)
            .OrderBy(r => r.CompletedAt)
            .ToListAsync();
    }

    // === Lesson Progress ===
    public async Task<LessonProgress?> GetLessonProgressAsync(int userId, int lessonId)
    {
        return await _context.LessonProgresses
            .FirstOrDefaultAsync(lp => lp.UserProfileId == userId && lp.LessonId == lessonId);
    }

    public async Task<List<LessonProgress>> GetAllLessonProgressAsync(int userId)
    {
        return await _context.LessonProgresses
            .Where(lp => lp.UserProfileId == userId)
            .ToListAsync();
    }

    public async Task UpdateLessonProgressAsync(int userId, int lessonId, double wpm, double accuracy)
    {
        var progress = await GetLessonProgressAsync(userId, lessonId);

        if (progress == null)
        {
            progress = new LessonProgress
            {
                UserProfileId = userId,
                LessonId = lessonId,
                BestWpm = wpm,
                BestAccuracy = accuracy,
                AttemptCount = 1,
                IsCompleted = accuracy >= 80,
                CompletedAt = accuracy >= 80 ? DateTime.UtcNow : null,
                LastAttemptAt = DateTime.UtcNow
            };
            _context.LessonProgresses.Add(progress);
        }
        else
        {
            progress.BestWpm = Math.Max(progress.BestWpm, wpm);
            progress.BestAccuracy = Math.Max(progress.BestAccuracy, accuracy);
            progress.AttemptCount++;
            progress.LastAttemptAt = DateTime.UtcNow;
            if (!progress.IsCompleted && accuracy >= 80)
            {
                progress.IsCompleted = true;
                progress.CompletedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
    }

    // === Statistics ===
    public async Task<(double avgWpm, double avgAccuracy, double bestWpm, int totalTests)> GetSummaryStatsAsync(int userId)
    {
        var results = await _context.TestResults
            .Where(r => r.UserProfileId == userId)
            .ToListAsync();

        if (results.Count == 0)
            return (0, 0, 0, 0);

        return (
            Math.Round(results.Average(r => r.Wpm), 1),
            Math.Round(results.Average(r => r.Accuracy), 1),
            Math.Round(results.Max(r => r.Wpm), 1),
            results.Count
        );
    }

    public async Task UpdateUsernameAsync(int userId, string username)
    {
        var user = await _context.UserProfiles.FindAsync(userId);
        if (user == null) return;
        user.Username = username;
        await _context.SaveChangesAsync();
    }

    public async Task ClearAllDataAsync(int userId)
    {
        var results = _context.TestResults.Where(r => r.UserProfileId == userId);
        _context.TestResults.RemoveRange(results);

        var progress = _context.LessonProgresses.Where(lp => lp.UserProfileId == userId);
        _context.LessonProgresses.RemoveRange(progress);

        var user = await _context.UserProfiles.FindAsync(userId);
        if (user != null)
        {
            user.TotalTestsTaken = 0;
            user.BestWpm = 0;
            user.AverageWpm = 0;
            user.AverageAccuracy = 0;
        }

        await _context.SaveChangesAsync();
    }
}
