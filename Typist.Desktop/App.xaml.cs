using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Typist.Desktop.Data;
using Typist.Desktop.Services;
using Typist.Desktop.ViewModels;

namespace Typist.Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Data
        services.AddDbContext<TypistDbContext>(ServiceLifetime.Singleton);
        services.AddSingleton<DatabaseService>();

        // ViewModels
        services.AddSingleton<MainViewModel>();

        // Main Window
        services.AddSingleton<MainWindow>();

        Services = services.BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
