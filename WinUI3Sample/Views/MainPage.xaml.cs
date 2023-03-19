
using Microsoft.UI.Xaml.Controls;
using WinUI3Sample.Models;
using WinUI3Sample.Services;
using WinUI3Sample.ViewModels;

namespace WinUI3Sample.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        this.DataContext = new KSAppData { MyName = "Hello world!" };
    }
   
}
