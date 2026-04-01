using System.Windows;
using System.Windows.Input;
using Typist.Desktop.Services;
using Typist.Desktop.ViewModels;

namespace Typist.Desktop;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _vm.InitializeAsync();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Escape → restart in test mode
        if (e.Key == Key.Escape && _vm.CurrentView is TestViewModel testVm && testVm.IsTyping)
        {
            testVm.RestartTestCommand.Execute(null);
            e.Handled = true;
            return;
        }

        // Backspace
        if (e.Key == Key.Back)
        {
            _vm.HandleBackspace();
            e.Handled = true;
        }
    }

    private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (e.Text.Length > 0)
        {
            _vm.HandleKeyPress(e.Text[0]);
            e.Handled = true;
        }
    }
}