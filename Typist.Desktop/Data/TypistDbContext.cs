using Microsoft.EntityFrameworkCore;
using System.IO;
using Typist.Desktop.Models;

namespace Typist.Desktop.Data;

public class TypistDbContext : DbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<TestResult> TestResults { get; set; } = null!;
    public DbSet<LessonProgress> LessonProgresses { get; set; } = null!;

    private readonly string _dbPath;

    public TypistDbContext()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var typistDir = Path.Combine(appData, "Typist");
        Directory.CreateDirectory(typistDir);
        _dbPath = Path.Combine(typistDir, "typist.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasOne(t => t.UserProfile)
                  .WithMany(u => u.TestResults)
                  .HasForeignKey(t => t.UserProfileId);

            entity.HasIndex(t => t.CompletedAt);
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.HasOne(lp => lp.Lesson)
                  .WithMany(l => l.Progresses)
                  .HasForeignKey(lp => lp.LessonId);

            entity.HasOne(lp => lp.UserProfile)
                  .WithMany(u => u.LessonProgresses)
                  .HasForeignKey(lp => lp.UserProfileId);

            entity.HasIndex(lp => new { lp.UserProfileId, lp.LessonId }).IsUnique();
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasIndex(l => l.OrderIndex);
        });
    }
}
