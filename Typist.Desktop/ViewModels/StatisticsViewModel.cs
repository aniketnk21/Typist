using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using Typist.Desktop.Models;
using Typist.Desktop.Services;

namespace Typist.Desktop.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;

    [ObservableProperty] private double _bestWpm;
    [ObservableProperty] private double _averageWpm;
    [ObservableProperty] private double _averageAccuracy;
    [ObservableProperty] private int _totalTests;
    [ObservableProperty] private bool _hasData;

    public ObservableCollection<TestResult> RecentResults { get; } = new();

    public ISeries[] ChartSeries { get; private set; } = Array.Empty<ISeries>();
    public Axis[] XAxes { get; private set; } = Array.Empty<Axis>();
    public Axis[] YAxes { get; private set; } = new[]
    {
        new Axis
        {
            Name = "WPM",
            NamePaint = new SolidColorPaint(SKColor.Parse("#00D2D6")),
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#B0B0C0")),
            TextSize = 11,
            SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#2D2D3D"))
        }
    };

    public StatisticsViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task LoadAsync(int userId)
    {
        var (avgWpm, avgAcc, bestWpm, total) = await _dbService.GetSummaryStatsAsync(userId);
        BestWpm = bestWpm;
        AverageWpm = avgWpm;
        AverageAccuracy = avgAcc;
        TotalTests = total;
        HasData = total > 0;

        var all = await _dbService.GetTestResultsAsync(userId, 20);
        RecentResults.Clear();
        foreach (var r in all)
            RecentResults.Add(r);

        BuildChart(all);
    }

    private void BuildChart(List<TestResult> results)
    {
        if (results.Count == 0)
        {
            ChartSeries = Array.Empty<ISeries>();
            XAxes = Array.Empty<Axis>();
            OnPropertyChanged(nameof(ChartSeries));
            OnPropertyChanged(nameof(XAxes));
            return;
        }

        var ordered = results.OrderBy(r => r.CompletedAt).ToList();
        var wpmValues = ordered.Select(r => r.Wpm).ToArray();
        var labels = ordered.Select(r => r.CompletedAt.ToString("MM/dd HH:mm")).ToArray();

        ChartSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = wpmValues,
                Name = "WPM",
                Stroke = new SolidColorPaint(SKColor.Parse("#00D2D6"), 2),
                Fill = new LinearGradientPaint(
                    new SKColor[] { SKColor.Parse("#3300D2D6"), SKColor.Parse("#0000D2D6") },
                    new SKPoint(0, 0), new SKPoint(0, 1)),
                GeometryStroke = new SolidColorPaint(SKColor.Parse("#00D2D6"), 2),
                GeometryFill = new SolidColorPaint(SKColor.Parse("#1A2535")),
                GeometrySize = 6,
                LineSmoothness = 0.5
            }
        };

        XAxes = new[]
        {
            new Axis
            {
                Labels = labels,
                LabelsPaint = new SolidColorPaint(SKColor.Parse("#B0B0C0")),
                TextSize = 10,
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#2D2D3D"))
            }
        };

        OnPropertyChanged(nameof(ChartSeries));
        OnPropertyChanged(nameof(XAxes));
    }
}
