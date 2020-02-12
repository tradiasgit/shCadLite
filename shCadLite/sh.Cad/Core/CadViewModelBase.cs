using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace sh.Cad
{

    public class CadViewModelBase : INotifyPropertyChanged
    {
        #region Busy
        public bool IsBusy { get { return GetValue<bool>(); } private set { SetValue(value); } }

        public string BusyMessage { get { return GetValue<string>(); } set { SetValue(value); } }

        public string BusyTitle { get { return GetValue<string>(); } private set {  SetValue(value); } }


        protected void SetBusy(string title, string message = "")
        {
            BusyTitle = title;
            BusyMessage = message;
            IsBusy = true;
        }
        protected void ClearBusy()
        {
            IsBusy = false;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Values

        protected Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

        protected T GetValue<T>([CallerMemberName]string name = null)
        {
            if (!Values.ContainsKey(name))
                return default(T);
            else return (T)Values[name];
        }
        protected void SetValue(object value, [CallerMemberName]string name = null)
        {
            if (!Values.ContainsKey(name))
                Values.Add(name, value);
            else Values[name] = value;
            RaisePropertyChanged(name);
        }

        #endregion

        #region cad

        protected Document doc { get { return Application.DocumentManager.MdiActiveDocument; } }

        protected Database db { get { return HostApplicationServices.WorkingDatabase; } }

        protected Editor ed { get { return Application.DocumentManager.MdiActiveDocument?.Editor; } }


        private static DirectoryInfo GetFileDirectory(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return null;
            else return new FileInfo(file).Directory;
        }


        protected DirectoryInfo DatabaseDirectory => GetFileDirectory(HostApplicationServices.WorkingDatabase?.OriginalFileName);

        protected DirectoryInfo AssemblyDirectory => GetFileDirectory(Assembly.GetExecutingAssembly().Location);


        public FileInfo GetFileInfo(DirectoryInfo dir, string name)
        {
            return new FileInfo($@"{dir.FullName}\{name}");
        }

        #endregion

        #region cadmethods

        protected void ShowMessage(string message)
        {
            Application.ShowAlertDialog(message);
        }
        protected bool Confirm(string message)
        {
            var result = System.Windows.MessageBox.Show(message, "确认", System.Windows.MessageBoxButton.YesNo);
            return result == System.Windows.MessageBoxResult.Yes;
        }

        protected virtual void OnException(Exception ex)
        {
            ShowMessage(ex.Message);
        }

        protected void WriteLine(string Message)
        {
            ed?.WriteMessage(Environment.NewLine + Message + Environment.NewLine);
        }

        #endregion

        #region commands

        protected ICommand RegisterCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            return new DelegateCommand(execute, canExecute);
        }

        protected ICommand RegisterCommandAsync(Func<object,Task> execute, Predicate<object> canExecute = null)
        {
            return new DelegateCommandAsync(execute, canExecute);
        }

        #endregion
    }

    internal class DelegateCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;
        protected void OnCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        protected readonly Predicate<object> _canExecute = p => true;
        protected readonly Action<object> _execute;

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            if (canExecute != null) _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            using (doc.LockDocument())
            {
                try
                {
                    if (_canExecute(parameter)) { _execute(parameter); ed.Regen(); }
                }
                catch (Exception ex)
                {
                    ed.WriteMessage($"{Environment.NewLine}【异常】{ex.Message}{Environment.NewLine}");
                }
            }
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }
    }




    internal class DelegateCommandAsync : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected void OnCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }
        protected readonly Predicate<object> _canExecute = p => true;
        protected readonly Func<object, Task> _execute;
        public DelegateCommandAsync(Func<object, Task> execute, Predicate<object> canExecute = null) 
        {
            _execute = execute;
            if(canExecute != null) _canExecute = canExecute;
        }

        public async void Execute(object parameter)
        {
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        public async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _execute?.Invoke(parameter);
            }
            catch (Exception e)
            {
                OnException(e);
            }

        }

        protected virtual void OnException(Exception ex)
        {
            ShowMessage(ex.Message);
        }
        protected void ShowMessage(string message)
        {
            Application.ShowAlertDialog(message);
        }

        //static DelegateCommandAsync()
        //{
        //    System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        //}

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            System.Windows.MessageBox.Show("【未处理UI异常】" + e.Exception);
        }

    }
}
