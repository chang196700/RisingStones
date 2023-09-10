using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RisingStones
{
    public class MainWindowViewModel
    {
        public MainWindow Window { get; private set; }

        public MainWindowViewModel(MainWindow window)
        {
            Window = window;
        }

        private ICommand? pageNavigateCommand;
        private ICommand? userLogoutCommand;

        public ICommand PageNavigateCommand
        {
            get
            {
                if (pageNavigateCommand == null)
                {
                    var command = new CommonCommand()
                    {
                        DoCanExecute = (para) =>
                        {
                            return para is string uri && Window.webView != null && Window.webView.CoreWebView2 != null && Window.webView.Source.ToString() != uri;
                        },
                        DoExecute = (para) =>
                        {
                            if (para is string uri)
                            {
                                Window.webView.CoreWebView2.Navigate(uri);
                            }
                        }
                    };

                    Window.SourceInitialized += (_, _) =>
                    {
                        Window.webView.SourceChanged += (sender, e) =>
                        {
                            command.DoCanExecuteChanged();
                        };
                    };

                    pageNavigateCommand = command;
                }

                return pageNavigateCommand;
            }
        }

        public ICommand UserLogoutCommand
        {
            get
            {
                if (userLogoutCommand == null)
                {
                    var command = new CommonCommand()
                    {
                        DoCanExecute = _=>
                        {
                            return Window.webView != null && Window.webView.CoreWebView2 != null;
                        },
                        DoExecute = _ =>
                        {
                            Window.webView.CoreWebView2.Navigate("https://apiff14risingstones.web.sdo.com/api/home/GHome/logOut?url=https://ff14risingstones.web.sdo.com/mob/index.html");
                        }
                    };

                    Window.SourceInitialized += (_, _) => Window.webView.SourceChanged += (_, _) => command.DoCanExecuteChanged();

                    userLogoutCommand = command;
                }

                return userLogoutCommand;
            }
            set => userLogoutCommand = value;
        }

        class CommonCommand : ICommand
        {
            public Func<object?, bool>? DoCanExecute { get; set; }
            public Action<object?>? DoExecute { get; set; }

            public void DoCanExecuteChanged()
            {
                canExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            public CommonCommand(Func<object?, bool>? doCanExecute = null, Action<object?>? doExecute = null)
            {
                DoCanExecute = doCanExecute;
                DoExecute = doExecute;
            }

            private EventHandler? canExecuteChanged;

            event EventHandler? ICommand.CanExecuteChanged
            {
                add
                {
                    canExecuteChanged += value;
                }

                remove
                {
                    canExecuteChanged -= value;
                }
            }

            bool ICommand.CanExecute(object? parameter)
            {
                return DoCanExecute?.Invoke(parameter) ?? true;
            }

            void ICommand.Execute(object? parameter)
            {
                DoExecute?.Invoke(parameter);
            }
        }
    }
}
