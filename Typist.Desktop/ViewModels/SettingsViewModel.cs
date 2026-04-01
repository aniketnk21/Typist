using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Typist.Desktop.Services;

namespace Typist.Desktop.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private int _userId;

    [ObservableProperty] private string _username = "Default";
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isStatusError;

    public SettingsViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task LoadAsync(int userId)
    {
        _userId = userId;
        var user = await _dbService.GetDefaultUserAsync();
        Username = user.Username;
    }

    [RelayCommand]
    private async Task SaveUsername()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            StatusMessage = "Username cannot be empty.";
            IsStatusError = true;
            return;
        }

        await _dbService.UpdateUsernameAsync(_userId, Username.Trim());
        StatusMessage = "Username saved!";
        IsStatusError = false;

        // Auto-clear message
        await Task.Delay(3000);
        StatusMessage = string.Empty;
    }

    [RelayCommand]
    private async Task ClearAllData()
    {
        var result = MessageBox.Show(
            "Are you sure you want to clear all your typing history and lesson progress? This cannot be undone.",
            "Clear All Data",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        await _dbService.ClearAllDataAsync(_userId);
        StatusMessage = "All data cleared.";
        IsStatusError = false;

        await Task.Delay(3000);
        StatusMessage = string.Empty;
    }
}
