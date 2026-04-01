using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Typist.Desktop.Services;

namespace Typist.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;

    [ObservableProperty]
    private ObservableObject? _currentView;

    [ObservableProperty]
    private string _currentViewName = "Dashboard";

    [ObservableProperty]
    private int _userId = 1;

    public DashboardViewModel DashboardVm { get; }
    public LessonViewModel LessonVm { get; }
    public TestViewModel TestVm { get; }
    public StatisticsViewModel StatisticsVm { get; }
    public SettingsViewModel SettingsVm { get; }

    public MainViewModel(DatabaseService dbService)
    {
        _dbService = dbService;

        DashboardVm = new DashboardViewModel(dbService, this);
        LessonVm = new LessonViewModel(dbService, this);
        TestVm = new TestViewModel(dbService, this);
        StatisticsVm = new StatisticsViewModel(dbService);
        SettingsVm = new SettingsViewModel(dbService);

        CurrentView = DashboardVm;
    }

    public async Task InitializeAsync()
    {
        await _dbService.InitializeAsync();
        var user = await _dbService.GetDefaultUserAsync();
        UserId = user.Id;
        TestVm.SetUserId(UserId);
        await DashboardVm.LoadAsync(UserId);
    }

    [RelayCommand]
    private async Task NavigateTo(string viewName)
    {
        CurrentViewName = viewName;

        switch (viewName)
        {
            case "Dashboard":
                CurrentView = DashboardVm;
                await DashboardVm.LoadAsync(UserId);
                break;
            case "Lessons":
                CurrentView = LessonVm;
                await LessonVm.LoadLessonsAsync(UserId);
                break;
            case "Test":
                CurrentView = TestVm;
                TestVm.SetUserId(UserId);
                TestVm.ResetToConfig();
                break;
            case "Statistics":
                CurrentView = StatisticsVm;
                await StatisticsVm.LoadAsync(UserId);
                break;
            case "Settings":
                CurrentView = SettingsVm;
                await SettingsVm.LoadAsync(UserId);
                break;
        }
    }

    public void HandleKeyPress(char key)
    {
        if (CurrentView is LessonViewModel lesson && lesson.IsTyping)
        {
            lesson.ProcessKey(key);
        }
        else if (CurrentView is TestViewModel test && test.IsTyping)
        {
            test.ProcessKey(key);
        }
    }

    public void HandleBackspace()
    {
        if (CurrentView is LessonViewModel lesson && lesson.IsTyping)
        {
            lesson.ProcessBackspace();
        }
        else if (CurrentView is TestViewModel test && test.IsTyping)
        {
            test.ProcessBackspace();
        }
    }
}
