using System.Windows.Controls;
using System.Windows.Input;
using Typist.Desktop.Models;
using Typist.Desktop.ViewModels;

namespace Typist.Desktop.Views;

public partial class LessonView : UserControl
{
    public LessonView()
    {
        InitializeComponent();
    }

    private void LessonItem_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is ListBoxItem item && item.DataContext is Lesson lesson && DataContext is LessonViewModel vm)
        {
            vm.SelectLessonCommand.Execute(lesson);
        }
    }
}
