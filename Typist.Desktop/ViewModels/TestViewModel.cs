using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Typist.Desktop.Data;
using Typist.Desktop.Models;
using Typist.Desktop.Services;

namespace Typist.Desktop.ViewModels;

public partial class TestViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly MainViewModel _mainVm;
    private readonly TypingEngine _engine = new();
    private readonly TimerService _timer = new();
    private int _userId;
    private List<string> _testTexts = SeedData.GetTestTexts();
    private int _currentTextIndex;

    // --- Config state ---
    [ObservableProperty] private bool _isConfiguring = true;
    [ObservableProperty] private bool _isTyping;
    [ObservableProperty] private bool _isFinished;
    [ObservableProperty] private TestType _selectedTestType = TestType.Timed1Min;
    [ObservableProperty] private int _selectedDurationSeconds = 60;

    // --- Typing state ---
    [ObservableProperty] private int _timerDisplay;   // countdown or elapsed
    [ObservableProperty] private double _currentWpm;
    [ObservableProperty] private double _currentAccuracy = 100;
    [ObservableProperty] private double _currentRawWpm;
    [ObservableProperty] private int _currentIndex;

    // --- Result state ---
    [ObservableProperty] private double _resultWpm;
    [ObservableProperty] private double _resultRawWpm;
    [ObservableProperty] private double _resultAccuracy;
    [ObservableProperty] private int _resultCorrectChars;
    [ObservableProperty] private int _resultIncorrectChars;
    [ObservableProperty] private int _resultDurationSeconds;
    [ObservableProperty] private string _resultGrade = "?";

    public ObservableCollection<CharDisplayItem> CharItems { get; } = new();

    public TestViewModel(DatabaseService dbService, MainViewModel mainVm)
    {
        _dbService = dbService;
        _mainVm = mainVm;

        _engine.StatsUpdated += (_, stats) =>
        {
            CurrentWpm = stats.Wpm;
            CurrentRawWpm = stats.RawWpm;
            CurrentAccuracy = stats.Accuracy;
            CurrentIndex = _engine.CurrentIndex;
            RefreshCharItems();
        };

        _engine.Completed += async (_, _) =>
        {
            if (!IsTyping) return;
            _timer.Stop();
            IsTyping = false;
            await FinishTestAsync();
        };

        _timer.Tick += (_, val) =>
        {
            TimerDisplay = val;
        };

        _timer.TimeUp += async (_, _) =>
        {
            if (!IsTyping) return;
            _engine.Stop();
            IsTyping = false;
            await FinishTestAsync();
        };
    }

    public void ResetToConfig()
    {
        IsConfiguring = true;
        IsTyping = false;
        IsFinished = false;
        CharItems.Clear();
        CurrentWpm = 0;
        CurrentAccuracy = 100;
        CurrentRawWpm = 0;
        _timer.Reset();
        _currentTextIndex = new Random().Next(_testTexts.Count);
    }

    [RelayCommand]
    private void SelectDuration(string durationParam)
    {
        switch (durationParam)
        {
            case "Quick":
                SelectedTestType = TestType.Quick;
                SelectedDurationSeconds = 0;
                break;
            case "1":
                SelectedTestType = TestType.Timed1Min;
                SelectedDurationSeconds = 60;
                break;
            case "2":
                SelectedTestType = TestType.Timed2Min;
                SelectedDurationSeconds = 120;
                break;
            case "5":
                SelectedTestType = TestType.Timed5Min;
                SelectedDurationSeconds = 300;
                break;
        }
    }

    [RelayCommand]
    private void StartTest()
    {
        _currentTextIndex = new Random().Next(_testTexts.Count);
        var text = _testTexts[_currentTextIndex];
        _engine.Initialize(text);
        RefreshCharItems();

        IsConfiguring = false;
        IsTyping = true;
        IsFinished = false;
        CurrentWpm = 0;
        CurrentAccuracy = 100;

        _timer.Reset();
        if (SelectedTestType == TestType.Quick)
        {
            _timer.StartElapsed();
            TimerDisplay = 0;
        }
        else
        {
            _timer.StartCountdown(SelectedDurationSeconds);
            TimerDisplay = SelectedDurationSeconds;
        }
    }

    [RelayCommand]
    private void RestartTest()
    {
        ResetToConfig();
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

    private async Task FinishTestAsync()
    {
        var stats = _engine.CalculateStats();
        int elapsed = SelectedTestType == TestType.Quick
            ? (int)_engine.GetElapsedSeconds()
            : SelectedDurationSeconds;

        ResultWpm = stats.Wpm;
        ResultRawWpm = stats.RawWpm;
        ResultAccuracy = stats.Accuracy;
        ResultCorrectChars = stats.CorrectChars;
        ResultIncorrectChars = stats.IncorrectChars;
        ResultDurationSeconds = elapsed;
        ResultGrade = GetGrade(stats.Wpm);

        var result = new TestResult
        {
            UserProfileId = _userId,
            Wpm = stats.Wpm,
            RawWpm = stats.RawWpm,
            Accuracy = stats.Accuracy,
            TotalCharacters = stats.TotalChars,
            CorrectCharacters = stats.CorrectChars,
            IncorrectCharacters = stats.IncorrectChars,
            TotalWords = stats.TotalWords,
            CorrectWords = stats.CorrectWords,
            DurationSeconds = elapsed,
            TestType = SelectedTestType,
            CompletedAt = DateTime.UtcNow
        };

        await _dbService.SaveTestResultAsync(result);
        _userId = _mainVm.UserId;

        IsTyping = false;
        IsFinished = true;
    }

    public void SetUserId(int userId) => _userId = userId;

    private static string GetGrade(double wpm) => wpm switch
    {
        >= 100 => "S",
        >= 80  => "A",
        >= 60  => "B",
        >= 40  => "C",
        >= 20  => "D",
        _      => "F"
    };

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
}
