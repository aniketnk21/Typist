namespace Typist.Desktop.Services;

public enum CharState
{
    Pending,
    Current,
    Correct,
    Incorrect
}

public class TypingStats
{
    public double Wpm { get; set; }
    public double RawWpm { get; set; }
    public double Accuracy { get; set; }
    public int CorrectChars { get; set; }
    public int IncorrectChars { get; set; }
    public int TotalTyped { get; set; }
    public int TotalChars { get; set; }
    public double ElapsedSeconds { get; set; }
    public int CorrectWords { get; set; }
    public int TotalWords { get; set; }
}

public class TypingEngine
{
    private string _targetText = string.Empty;
    private readonly List<CharState> _charStates = new();
    private readonly List<char> _typedChars = new();
    private int _currentIndex;
    private readonly System.Diagnostics.Stopwatch _stopwatch = new();
    private bool _isStarted;
    private bool _isCompleted;

    public event EventHandler<TypingStats>? StatsUpdated;
    public event EventHandler? Completed;

    public string TargetText => _targetText;
    public IReadOnlyList<CharState> CharStates => _charStates.AsReadOnly();
    public int CurrentIndex => _currentIndex;
    public bool IsStarted => _isStarted;
    public bool IsCompleted => _isCompleted;

    public void Initialize(string text)
    {
        _targetText = text;
        _charStates.Clear();
        _typedChars.Clear();
        _currentIndex = 0;
        _isStarted = false;
        _isCompleted = false;
        _stopwatch.Reset();

        for (int i = 0; i < text.Length; i++)
        {
            _charStates.Add(i == 0 ? CharState.Current : CharState.Pending);
        }
    }

    public void ProcessKeyPress(char key)
    {
        if (_isCompleted || _currentIndex >= _targetText.Length)
            return;

        if (!_isStarted)
        {
            _isStarted = true;
            _stopwatch.Start();
        }

        _typedChars.Add(key);

        // Compare with expected character
        if (key == _targetText[_currentIndex])
        {
            _charStates[_currentIndex] = CharState.Correct;
        }
        else
        {
            _charStates[_currentIndex] = CharState.Incorrect;
        }

        _currentIndex++;

        // Set current marker on next char
        if (_currentIndex < _targetText.Length)
        {
            _charStates[_currentIndex] = CharState.Current;
        }

        // Fire stats update
        var stats = CalculateStats();
        StatsUpdated?.Invoke(this, stats);

        // Check completion
        if (_currentIndex >= _targetText.Length)
        {
            _isCompleted = true;
            _stopwatch.Stop();
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool ProcessBackspace()
    {
        if (_currentIndex <= 0 || _isCompleted)
            return false;

        // Remove current marker
        if (_currentIndex < _targetText.Length)
        {
            _charStates[_currentIndex] = CharState.Pending;
        }

        _currentIndex--;
        _charStates[_currentIndex] = CharState.Current;

        if (_typedChars.Count > 0)
        {
            _typedChars.RemoveAt(_typedChars.Count - 1);
        }

        var stats = CalculateStats();
        StatsUpdated?.Invoke(this, stats);
        return true;
    }

    public TypingStats CalculateStats()
    {
        double elapsedSeconds = _stopwatch.Elapsed.TotalSeconds;
        if (elapsedSeconds < 0.1) elapsedSeconds = 0.1; // prevent division by zero

        int correctChars = _charStates.Count(c => c == CharState.Correct);
        int incorrectChars = _charStates.Count(c => c == CharState.Incorrect);
        int totalTyped = correctChars + incorrectChars;

        double elapsedMinutes = elapsedSeconds / 60.0;

        // Standard WPM: (correct chars / 5) / minutes
        double rawWpm = (totalTyped / 5.0) / elapsedMinutes;
        double netWpm = Math.Max(0, (correctChars / 5.0) / elapsedMinutes);

        double accuracy = totalTyped > 0 ? (correctChars / (double)totalTyped) * 100 : 100;

        // Count words
        var words = _targetText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int totalWords = words.Length;
        int correctWords = CountCorrectWords();

        return new TypingStats
        {
            Wpm = Math.Round(netWpm, 1),
            RawWpm = Math.Round(rawWpm, 1),
            Accuracy = Math.Round(accuracy, 1),
            CorrectChars = correctChars,
            IncorrectChars = incorrectChars,
            TotalTyped = totalTyped,
            TotalChars = _targetText.Length,
            ElapsedSeconds = elapsedSeconds,
            CorrectWords = correctWords,
            TotalWords = totalWords
        };
    }

    private int CountCorrectWords()
    {
        if (_currentIndex == 0) return 0;

        // Split target text into words and check if each word was typed correctly
        int correctWords = 0;
        int charIndex = 0;

        var words = _targetText.Split(' ');
        foreach (var word in words)
        {
            if (charIndex + word.Length > _currentIndex)
                break;

            bool wordCorrect = true;
            for (int i = 0; i < word.Length; i++)
            {
                if (_charStates[charIndex + i] != CharState.Correct)
                {
                    wordCorrect = false;
                    break;
                }
            }

            if (wordCorrect) correctWords++;
            charIndex += word.Length + 1; // +1 for space
        }

        return correctWords;
    }

    public void Stop()
    {
        _stopwatch.Stop();
        _isCompleted = true;
    }

    public double GetElapsedSeconds() => _stopwatch.Elapsed.TotalSeconds;
}
