using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.Graphics;
using Windows.System;
using WinRT.Interop;
using Microsoft.Extensions.Configuration;
using WinRT;
using System.Drawing;

namespace WinUI3Sample.Services;

//需要引入Pinvoke.User32、CommunityToolkit.WinUI、WinUIEx包

public class KSAppService
{
    /// <summary>
    /// App名称
    /// </summary>
    public string ApplicationName = SystemInformation.Instance.ApplicationName;
    /// <summary>
    /// App版本
    /// </summary>
    public PackageVersion ApplicationVersion = SystemInformation.Instance.ApplicationVersion;
    /// <summary>
    /// App是否第一次运行
    /// </summary>
    public bool IsFirstRun = SystemInformation.Instance.IsFirstRun;
    /// <summary>
    /// 得到屏幕工作区高度
    /// </summary>
    public double ScreenWorkAreaHeight = DisplayArea.Primary.WorkArea.Height;
    /// <summary>
    /// 得到屏幕工作区宽度
    /// </summary>
    public double ScreenWorkAreaWidth = DisplayArea.Primary.WorkArea.Width;
    /// <summary>
    /// 得到屏幕高度
    /// </summary>
    public double ScreenHeight = DisplayArea.Primary.OuterBounds.Height;
    /// <summary>
    /// 得到屏幕宽度
    /// </summary>
    public double ScreenWidth = DisplayArea.Primary.OuterBounds.Width;
    /// <summary>
    /// 窗口最大化
    /// </summary>
    public void WindowMax(Window window)
    {
        PInvoke.User32.ShowWindow(GetHwnd(window), PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
    }
    /// <summary>
    /// 窗口最小化
    /// </summary>
    public void WindowMin(Window window)
    {
        PInvoke.User32.ShowWindow(GetHwnd(window), PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
    }
    /// <summary>
    /// 窗口恢复
    /// </summary>
    public void WindowNormal(Window window)
    {
        PInvoke.User32.ShowWindow(GetHwnd(window), PInvoke.User32.WindowShowStyle.SW_RESTORE);
    }
    /// <summary>
    /// 以窗口最近一次的大小和状态显示窗口
    /// </summary>
    public void WindowLastRect(Window window)
    {
        PInvoke.User32.ShowWindow(GetHwnd(window), PInvoke.User32.WindowShowStyle.SW_SHOWNOACTIVATE);
    }
    /// <summary>
    /// 窗口隐藏
    /// </summary>
    public void WindowHide(Window window)
    {
        PInvoke.User32.ShowWindow(GetHwnd(window), PInvoke.User32.WindowShowStyle.SW_HIDE);
    }
    /// <summary>
    /// 窗口显示
    /// </summary>
    public void WindowShow(Window window)
    {
        PInvoke.User32.ShowWindow(GetHwnd(window), PInvoke.User32.WindowShowStyle.SW_SHOW);
    }
    /// <summary>
    /// 窗口置顶
    /// </summary>
    public void WindowIsAlwaysOnTop(Window window, bool IsAlwaysOnTop)
    {
        WindowManager.Get(window).IsAlwaysOnTop = IsAlwaysOnTop;
    }
    /// <summary>
    /// 窗口是否可改变大小
    /// </summary>
    public void WindowIsResizable(Window window, bool IsResizable)
    {
        WindowManager.Get(window).IsResizable = IsResizable;
    }
    /// <summary>
    /// 窗口全屏
    /// </summary>
    public void WindowFullScreen(Window window,bool IsFullScreen)
    {
        if (IsFullScreen)
            GetAppWindowForCurrentWindow(window).SetPresenter(AppWindowPresenterKind.FullScreen);
        else
            GetAppWindowForCurrentWindow(window).SetPresenter(AppWindowPresenterKind.Default);
    }
    /// <summary>
    /// 是否显示最大化按钮
    /// </summary>
    public void WindowIsMaximizable(Window window, bool a)
    {
        WindowManager.Get(window).IsMaximizable = a;
    }
    /// <summary>
    /// 是否显示最小化按钮
    /// </summary>
    public void WindowIsMinimizable(Window window, bool a)
    {
        WindowManager.Get(window).IsMinimizable = a;
    }
    /// <summary>
    /// 设置窗口大小（左上宽高）
    /// </summary>
    /// <param name="rect"></param>
    public void SetWindowRect(Window window,RectInt32 rect)
    {
        GetAppWindowForCurrentWindow(window).MoveAndResize(rect);
    }
    /// <summary>
    /// 得到窗口宽度
    /// </summary>
    public double WindowWidth(Window window)
    {
        return WindowManager.Get(window).Width;
    }
    /// <summary>
    /// 得到窗口高度
    /// </summary>
    public double WindowHeight(Window window)
    {
        return WindowManager.Get(window).Height;
    }
    /// <summary>
    /// 得到窗口左部
    /// </summary>
    public double WindowLeft(Window window)
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(window),out rect);
        return rect.left;
    }
    /// <summary>
    /// 得到窗口顶部
    /// </summary>
    public double WindowTop(Window window)
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(window), out rect);
        return rect.top;
    }
    /// <summary>
    /// 得到窗口右部
    /// </summary>
    public double WindowRight(Window window)
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(window), out rect);
        return rect.right;
    }
    /// <summary>
    /// 得到窗口底部
    /// </summary>
    public double WindowBottom(Window window)
    {
        PInvoke.RECT rect;
        PInvoke.User32.GetWindowRect(GetHwnd(window), out rect);
        return rect.bottom;
    }
    /// <summary>
    /// 得到鼠标坐标
    /// </summary>
    public Point GetCursor(Window window)
    {
        PInvoke.POINT p;
        PInvoke.User32.GetCursorPos(out p);
        return p;
    }
    /// <summary>
    /// 设置窗口宽度
    /// </summary>
    public double SetWindowWidth(Window window, double w)
    {
        return WindowManager.Get(window).Width = w;
    }
    /// <summary>
    /// 设置窗口高度
    /// </summary>
    public double SetWindowHeight(Window window, double h)
    {
        return WindowManager.Get(window).Height = h;
    }
    /// <summary>
    /// 得到窗口句柄
    /// </summary>
    /// <returns></returns>
    public nint GetHwnd(Window window)
    {
        return WinRT.Interop.WindowNative.GetWindowHandle(window);
    }
    /// <summary>
    ///  LoadIcon("Images/windowIcon.ico");
    /// </summary>
    /// <param name="iconName"></param>
    public void LoadIcon(Window window,string iconName)
    {
        var hwnd = GetHwnd(window);
        var hIcon = PInvoke.User32.LoadImage(IntPtr.Zero, iconName, PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);
        PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
    }
    /// <summary>
    /// AppWindow
    /// </summary>
    /// <returns></returns>
    public AppWindow GetAppWindowForCurrentWindow(Window window)
    {
        WindowId myWndId = Win32Interop.GetWindowIdFromWindow(GetHwnd(window));
        return AppWindow.GetFromWindowId(myWndId);
    }
    /// <summary>
    /// 设置App标题
    /// </summary>
    public void SetAppTitle(Window window,string title)
    {
        var m_appWindow = GetAppWindowForCurrentWindow(window);
        if (m_appWindow != null)
        {
            m_appWindow.Title = title;
        }
    }
    /// <summary>
    /// 窗口内容是否拓展到标题栏
    /// </summary>
    /// <param name="window"></param>
    /// <param name="Isextend"></param>
    public void SetExtendstoTitleBar(Window window, bool Isextend)
    {
        window.ExtendsContentIntoTitleBar = Isextend;
    }
    WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See below for implementation.
    MicaController m_backdropController;
    SystemBackdropConfiguration m_configurationSource;
    /// <summary>
    /// 设置窗口亚克力效果
    /// </summary>
    bool TrySetSystemBackdrop(Window window)
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            m_configurationSource = new SystemBackdropConfiguration();

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme(window);

            m_backdropController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_backdropController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }
    void SetConfigurationSourceTheme(Window window)
    {
        switch (((FrameworkElement)window.Content).ActualTheme)
        {
            case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
        }
    }
    class WindowsSystemDispatcherQueueHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

        object m_dispatcherQueueController = null;
        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (m_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
            }
        }
    }

}
