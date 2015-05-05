using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using ManagementGui.Utils;


namespace ManagementGui.View.Control
{
    /// <summary>
    /// Логика взаимодействия для ProgressButton.xaml
    /// </summary>
    public partial class ProgressButton : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ProgressButton()
        {
            InitializeComponent();
           // DataContext = this;
           // ControlContent = "ProgressButtonControl";
            MetroProgressBar.Visibility = Visibility.Hidden;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ControlCommand == null) return;
                MetroProgressBar.Visibility = Visibility.Visible;
                Button.IsEnabled = false;
                //Action act = AsyncExecuteCommand;
                ControlCommand.Execute(null);
               
                MetroProgressBar.Visibility = Visibility.Hidden;
                Button.IsEnabled = true;


            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void AsyncExecuteCommand()
        {
            ControlCommand.Execute(null); 
        }


        public static readonly DependencyProperty Command = DependencyProperty.Register(
            "ControlCommand", typeof(ICommand), typeof(ProgressButton)
            , new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = false,
            });


        public ICommand ControlCommand
        {
            get { return (ICommand)GetValue(Command); }
            set
            {
                SetValue(Command, value);
                OnPropertyChanged("ControlCommand");
            }
        }

        public static DependencyProperty Text = DependencyProperty.Register(
            "ControlContent", typeof(string), typeof(ProgressButton), new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = false
            });

        //public static readonly DependencyProperty ProgressVisibilityProperty = DependencyProperty.Register("ProgressVisibility", typeof(Visibility), typeof(ProgressButton));
        //public static readonly DependencyProperty IsButtonEnabledProperty = DependencyProperty.Register("IsButtonEnabled", typeof (bool), typeof (ProgressButton));


        public string ControlContent
        {
            get
            {
                return (string)GetValue(Text);
            }
            set
            {
                SetValue(Text, value);
            }

        }

        //public Visibility ProgressVisibility
        //{
        //    get { return (Visibility)GetValue(ProgressVisibilityProperty); }
        //    set { SetValue(ProgressVisibilityProperty, value); }
        //}

        //public bool IsButtonEnabled
        //{
        //    get { return (bool) GetValue(IsButtonEnabledProperty); }
        //    set { SetValue(IsButtonEnabledProperty, value); }
        //}
    }

    public class AsyncCommand : ICommand, IDisposable
    {
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
        private readonly Func<bool> _canExecute;

        public AsyncCommand(Action action, Func<bool> canExecute = null, Action<object> completed = null,
                            Action<Exception> error = null)
        {
            _backgroundWorker.DoWork += (s, e) =>
            {
                CommandManager.InvalidateRequerySuggested();
                action();
            };

            _backgroundWorker.RunWorkerCompleted += (s, e) =>
            {
                if (completed != null && e.Error == null)
                    completed(e.Result);

                if (error != null && e.Error != null)
                    error(e.Error);

                CommandManager.InvalidateRequerySuggested();
            };

            _canExecute = canExecute;
        }

        public void Cancel()
        {
            if (_backgroundWorker.IsBusy)
                _backgroundWorker.CancelAsync();
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //return _canExecute == null
            //           ? !_backgroundWorker.IsBusy
            //           : !_backgroundWorker.IsBusy && _canExecute();
        }

        public void Execute(object parameter)
        {
            _backgroundWorker.RunWorkerAsync();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_backgroundWorker != null)
                    _backgroundWorker.Dispose();
            }
        }
    }
}
