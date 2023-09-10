using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace RisingStones
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowAspectRatio? aspectRatio;
        private DevToolsProtocolHelper? devToolshelper;
        private bool isEmulateRightClick = false;

        private readonly string userAgentMobile = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36 Edg/116.0.1938.69";

        private readonly string userDataFolder = Path.Combine(Helper.GetUserAppDataPath(), "UserData");

        private readonly string[] trustHost = new string[]
        {
            "ff14risingstones.web.sdo.com",
            "apiff14risingstones.web.sdo.com",
            "cas.sdo.com",
            "login.sdo.com",
            "login.u.sdo.com",
        };

        private readonly string injectScript = @"
window.originOnError = window.onerror; 
window.onerror = function(event, source, lineno, colno, error) {
    if (window.originOnError) window.originOnError(event, source, lineno, colno, error);
    return true;
}
";

        private CoreWebView2CreationProperties DefaultCoreWebView2CreationProperties
        {
            get
            {
                return new()
                {
                    UserDataFolder = userDataFolder,
                };
            }
        }

        public MainWindow()
        {
            DataContext = new MainWindowViewModel(this);

            InitializeComponent();

            webView.CreationProperties = DefaultCoreWebView2CreationProperties;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            webView.EnsureCoreWebView2Async();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper((Window)sender).Handle;
            var value = NativeMethod.GetWindowLong(hwnd, NativeMethod.GWL_STYLE);
            _ = NativeMethod.SetWindowLong(hwnd, NativeMethod.GWL_STYLE, value & ~NativeMethod.WS_MAXIMIZEBOX);

            aspectRatio = new WindowAspectRatio(this);
        }

        private void webView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            webView.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
            webView.CoreWebView2.ScriptDialogOpening += CoreWebView2_ScriptDialogOpening;

            webView.CoreWebView2.Settings.UserAgent = userAgentMobile;

            webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
            webView.CoreWebView2.Settings.IsPinchZoomEnabled = true;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
            webView.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
            webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;

            var ratio = webView.Width / webView.Height;

            devToolshelper = webView.CoreWebView2.GetDevToolsProtocolHelper();

            _ = devToolshelper.Emulation.SetTouchEmulationEnabledAsync(true);
            _ = devToolshelper.Emulation.SetEmitTouchEventsForMouseAsync(true);
            _ = devToolshelper.Emulation.SetScrollbarsHiddenAsync(true);

#if DEBUG
            webView.CoreWebView2.OpenDevToolsWindow();
#endif

            webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(injectScript);

            webView.CoreWebView2.Navigate("https://ff14risingstones.web.sdo.com/mob/index.html");
        }

        private void CoreWebView2_ScriptDialogOpening(object? sender, CoreWebView2ScriptDialogOpeningEventArgs e)
        {
        }

        private void CoreWebView2_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
            if (devToolshelper != null && !isEmulateRightClick)
            {
                int x = e.Location.X;
                int y = e.Location.Y;

                devToolshelper.Emulation.SetEmitTouchEventsForMouseAsync(false)
                    .ContinueWith(t =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            isEmulateRightClick = true;
                            return devToolshelper.Input.DispatchMouseEventAsync("mousePressed", x, y, button: "left");
                        }).ContinueWith(t =>
                        {
                            Task.Delay(100).ContinueWith(t =>
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    return devToolshelper.Input.DispatchMouseEventAsync("mouseReleased", x, y, button: "left");
                                }).ContinueWith(t =>
                                {
                                    Task.Delay(100).ContinueWith(t =>
                                    {
                                        isEmulateRightClick = false;
                                        Dispatcher.Invoke(() =>
                                        {
                                            return devToolshelper.Emulation.SetEmitTouchEventsForMouseAsync(true);
                                        });
                                    });
                                });
                            });
                        });
                    });
                    
                e.Handled = true;

                return;
            }

            if (e.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.Image)
            {
                CoreWebView2ContextMenuItem menuItem = webView.CoreWebView2.Environment.CreateContextMenuItem("Preview Image", null, CoreWebView2ContextMenuItemKind.Command);

                menuItem.CustomItemSelected += (sender, ex) =>
                {
                    string uri = e.ContextMenuTarget.SourceUri;
                    string referer = new UriBuilder(e.ContextMenuTarget.FrameUri)
                    {
                        Fragment = string.Empty,
                        Path = string.Empty,
                        Query = string.Empty,
                        UserName = string.Empty,
                        Password = string.Empty,
                    }.ToString();

                    var win = new Window();
                    var w = new WebView2();
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Content = w;
                    win.Closed += (sender, e) =>
                    {
                        w.Dispose();
                    };
                    w.CreationProperties = DefaultCoreWebView2CreationProperties;
                    w.EnsureCoreWebView2Async();
                    w.CoreWebView2InitializationCompleted += (o, arg) =>
                    {
                        w.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
                        w.CoreWebView2.WebResourceRequested += (o, arg) =>
                        {
                            arg.Request.Headers.SetHeader("Referer", referer);
                        };
                        w.CoreWebView2.Navigate(uri);
                    };

                    win.Show();
                };

                e.MenuItems.Insert(0, menuItem);
            }
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            var uri = new Uri(e.Uri);

            if (uri == null)
            {
                e.Handled = true;
                return;
            }

            //if (!trustHost.Contains(uri.Host))
            {
                e.Handled = true;
                Helper.OpenInDefaultBrowser(new(e.Uri));
                return;
            }
        }

        private void MenuAspectRatio_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;

            var ratioStr = menuItem.Header.ToString()?.Split(':');

            if (aspectRatio != null && ratioStr != null && ratioStr.Length == 2)
            {
                if (!double.TryParse(ratioStr[0], out var ratioX)) return;
                if (!double.TryParse(ratioStr[1], out var ratioY)) return;

                aspectRatio.Ratio = ratioX / ratioY;

                Height = Width / aspectRatio.Ratio;
            }
        }

        private void webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
            }
        }

        private void webView_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            var uri = new Uri(e.Uri);

            if (uri == null)
            {
                e.Cancel = true;
                return;
            }

            if (!trustHost.Contains(uri.Host))
            {
                e.Cancel = true;
                Helper.OpenInDefaultBrowser(uri);
                return;
            }
        }
    }
}
