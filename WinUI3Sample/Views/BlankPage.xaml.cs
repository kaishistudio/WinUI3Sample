using Microsoft.UI.Xaml.Controls;
using WinUI3Sample.Services;
using WinUI3Sample.ViewModels;

namespace WinUI3Sample.Views;

public sealed partial class BlankPage : Page
{
    public BlankViewModel ViewModel
    {
        get;
    }

    public BlankPage()
    {
        ViewModel = App.GetService<BlankViewModel>();
        InitializeComponent();

    }

}
