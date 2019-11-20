using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.UI.Common.MVVM
{
    public abstract class DelegateCommandBase<T> : ViewModelBase, IDelegateCommand, ICommand
    {
        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        protected readonly Predicate<T> _canExecute;


        protected virtual bool IsAuthorized
        {
            get
            {
                var file = AuthorizationFileName;
                if (!File.Exists(file)) return true;
                var lines = File.ReadAllLines(file);
                return lines.Any(l => l == Name);
            }
        }

        private string _authorizationFileName;
        public override string AuthorizationFileName
        {
            get
            {
                if (_authorizationFileName != null) return _authorizationFileName;
                else return Parent?.AuthorizationFileName;
            }
            set
            {
                _authorizationFileName = value;
            }
        }

        public DelegateCommandBase(Predicate<T> canExecute = null)
        {
            _canExecute = canExecute;
        }

        public virtual bool CanExecute(object parameter)
        {
            return IsAuthorized && (_canExecute?.Invoke((T)parameter) ?? true);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract void Execute(object parameter);

        public override string ToString()
        {
            return $"Command:{Name}";
        }
    }

    public class DelegateCommand<T> : DelegateCommandBase<T>
    {
        protected readonly Action<T> _execute;
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null) : base(canExecute)
        {
            _execute = execute;
        }


        public override void Execute(object parameter)
        {
            try
            {
                _execute?.Invoke((T)parameter);
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

    }

    public class DelegateCommandAsync<T> : DelegateCommandBase<T>
    {
        protected readonly Func<T, Task> _execute;
        public DelegateCommandAsync(Func<T, Task> execute, Predicate<T> canExecute = null) : base(canExecute)
        {
            _execute = execute;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                await ExecuteAsync((T)parameter);
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        public async Task ExecuteAsync(T parameter)
        {
            try
            {
                IsExecuting = true;
                await _execute?.Invoke(parameter);
            }
            catch (Exception e)
            {
                OnException(e);
            }
            finally
            {
                IsExecuting = false;
            }

        }

        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }

        public bool IsExecuting
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); RaiseCanExecuteChanged(); }
        }


        static DelegateCommandAsync()
        {
            System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            System.Windows.MessageBox.Show("【未处理UI异常】" + e.Exception);
        }

    }



}
