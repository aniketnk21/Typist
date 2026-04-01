using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Typist.Desktop.Models;
using Typist.Desktop.Services;

namespace Typist.Desktop.ViewModels;

public class CharDisplayItem
{
    public char Character { get; set; }
    public CharState State { get; set; }
    public bool IsCurrent { get; set; }
}

public partial class LessonViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly MainViewModel _mainVm;
    private readonly TypingEngine _engine = new();
    private int _userId;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private Lesson? _selectedLesson;
    [ObservableProperty] private bool _isTyping;
    [ObservableProperty] private bool _isCompleted;
    [ObservableProperty] private double _currentWpm;
    [ObservableProperty] private double _currentAccuracy;
    [ObservableProperty] private int _currentIndex;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private string _selectedDifficulty = "All";
    [ObservableProperty] private string _selectedCategory = "All";

    public ObservableCollection<Lesson> Lessons { get; } = new();
    public ObservableCollection<CharDisplayItem> CharItems { get; } = new();

    public List<string> Difficulties { get; } = new() { "All", "Beginner", "Intermediate", "Advanced" };
    public List<string> Categories { get; } = new() { "All", "HomeRow", "TopRow", "BottomRow", "Numbers", "CommonWords", "Sentences", "Paragraphs", "CodeSnippets" };

    public LessonViewModel(DatabaseService dbService, MainViewModel mainVm)
    {
        _dbService = dbService;
        _mainVm = mainVm;

        _engine.StatsUpdated += (_, stats) =>
        {
            CurrentWpm = stats.Wpm;
            CurrentAccuracy = stats.Accuracy;
            CurrentIndex = _engine.CurrentIndex;
            RefreshCharItems();
        };

        _engine.Completed += async (_, _) =>
        {
            IsTyping = false;
            IsCompleted = true;
            var stats = _engine.CalculateStats();
            StatusMessage = $"Completed! {stats.Wpm} WPM  ·  {stats.Accuracy}% accuracy";

            if (SelectedLesson != null)
            {
                await _dbService.UpdateLessonProgressAsync(_userId, SelectedLesson.Id, stats.Wpm, stats.Accuracy);
            }
        };
    }

    public async Task LoadLessonsAsync(int userId)
    {
        _userId = userId;
        IsLoading = true;
        var all = await _dbService.GetLessonsAsync();
        var progresses = await _dbService.GetAllLessonProgressAsync(userId);

        Lessons.Clear();
        foreach (var lesson in all)
            Lessons.Add(lesson);

        IsLoading = false;
    }

    [RelayCommand]
    private void SelectLesson(Lesson lesson)
    {
        SelectedLesson = lesson;
        IsTyping = false;
        IsCompleted = false;
        StatusMessage = string.Empty;
        CurrentWpm = 0;
        CurrentAccuracy = 0;
        CharItems.Clear();
    }

    [RelayCommand]
    private void StartLesson()
    {
        if (SelectedLesson == null) return;
        _engine.Initialize(SelectedLesson.Content);
        IsTyping = true;
        IsCompleted = false;
        StatusMessage = "Start typing…";
        CurrentWpm = 0;
        CurrentAccuracy = 100;
        RefreshCharItems();
    }

    [RelayCommand]
    private void ResetLesson()
    {
        if (SelectedLesson == null) return;
        _engine.Initialize(SelectedLesson.Content);
        IsTyping = true;
        IsCompleted = false;
        StatusMessage = "Start typing…";
        CurrentWpm = 0;
        CurrentAccuracy = 100;
        RefreshCharItems();
    }

    public void ProcessKey(char key)
    {
        _engine.ProcessKeyPress(key);
    }

    public void ProcessBackspace()
    {
        _engine.ProcessBackspace();
        RefreshCharItems();
    }

    private void RefreshCharItems()
    {
        var text = _engine.TargetText;
        var states = _engine.CharStates;
        var idx = _engine.CurrentIndex;

        CharItems.Clear();
        for (int i = 0; i < text.Length; i++)
        {
            CharItems.Add(new CharDisplayItem
            {
                Character = text[i],
                State = i < states.Count ? states[i] : CharState.Pending,
                IsCurrent = i == idx
            });
        }
    }

    partial void OnSelectedDifficultyChanged(string value) => ApplyFilters();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilters();

    private async void ApplyFilters()
    {
        Difficulty? diff = SelectedDifficulty == "All" ? null
            : Enum.TryParse<Difficulty>(SelectedDifficulty, out var d) ? d : null;

        LessonCategory? cat = SelectedCategory == "All" ? null
            : Enum.TryParse<LessonCategory>(SelectedCategory, out var c) ? c : null;

        var filtered = await _dbService.GetLessonsAsync(diff, cat);
        Lessons.Clear();
        foreach (var l in filtered)
            Lessons.Add(l);
    }
}
