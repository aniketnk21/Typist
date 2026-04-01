using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Typist.Desktop.Models;
using Typist.Desktop.Services;

namespace Typist.Desktop.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly MainViewModel _mainVm;

    [ObservableProperty] private double _bestWpm;
    [ObservableProperty] private double _averageWpm;
    [ObservableProperty] private double _averageAccuracy;
    [ObservableProperty] private int _totalTests;
    [ObservableProperty] private int _lessonsCompleted;
    [ObservableProperty] private int _totalLessons;
    [ObservableProperty] private double _lessonProgress;
    [ObservableProperty] private bool _isEmpty = true;

    public ObservableCollection<TestResult> RecentResults { get; } = new();

    public DashboardViewModel(DatabaseService dbService, MainViewModel mainVm)
    {
        _dbService = dbService;
        _mainVm = mainVm;
    }

    public async Task LoadAsync(int userId)
    {
        var (avgWpm, avgAcc, bestWpm, total) = await _dbService.GetSummaryStatsAsync(userId);
        BestWpm = bestWpm;
        AverageWpm = avgWpm;
        AverageAccuracy = avgAcc;
        TotalTests = total;
        IsEmpty = total == 0;

        var recent = await _dbService.GetTestResultsAsync(userId, 5);
        RecentResults.Clear();
        foreach (var r in recent)
            RecentResults.Add(r);

        var allProgress = await _dbService.GetAllLessonProgressAsync(userId);
        LessonsCompleted = allProgress.Count(p => p.IsCompleted);

        var lessons = await _dbService.GetLessonsAsync();
        TotalLessons = lessons.Count;
        LessonProgress = TotalLessons > 0 ? (LessonsCompleted / (double)TotalLessons) * 100 : 0;
    }

    [RelayCommand]
    private async Task StartQuickTest()
    {
        await _mainVm.NavigateToCommand.ExecuteAsync("Test");
    }

    [RelayCommand]
    private async Task GoToLessons()
    {
        await _mainVm.NavigateToCommand.ExecuteAsync("Lessons");
    }
}
