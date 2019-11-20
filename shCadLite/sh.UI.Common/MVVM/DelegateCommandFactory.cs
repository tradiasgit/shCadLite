using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sh.UI.Common.MVVM
{
    public class DelegateCommandFactory
    {
        internal DelegateCommandFactory(IViewModelBase vm) { ViewModel = vm; }

        private IViewModelBase ViewModel { get; }
        protected Dictionary<string, IDelegateCommand> CommandList { get; } = new Dictionary<string, IDelegateCommand>();
        public void RaiseCommandAvailable()
        {
            foreach (var cmd in CommandList.Values)
            {
                if (cmd != null) cmd.RaiseCanExecuteChanged();
            }
        }

        public IDelegateCommand RegisterCommand<TParam>(IDelegateCommand command, [CallerMemberName]string commandName = null)
        {
            if (!CommandList.ContainsKey(commandName))
                CommandList.Add(commandName, command);
            command.Parent = ViewModel;
            command.Name = commandName;
            return CommandList[commandName];
        }
        public IDelegateCommand RegisterCommand(IDelegateCommand command, [CallerMemberName]string commandName = null)
        {
            return RegisterCommand<object>(command, commandName);
        }
        public IDelegateCommand RegisterCommand<TParam>(Action<TParam> execute, Predicate<TParam> canExecute = null, [CallerMemberName]string commandName = null)
        {
            return RegisterCommand<TParam>(new DelegateCommand<TParam>(execute, canExecute), commandName);
        }
        public IDelegateCommand RegisterCommand(Action<object> execute, Predicate<object> canExecute = null, [CallerMemberName]string commandName = null)
        {
            return RegisterCommand<object>(execute, canExecute, commandName);
        }


        public IDelegateCommand RegisterCommand<TParam>(Func<TParam, Task> execute, Predicate<TParam> canExecute = null, [CallerMemberName]string commandName = null)
        {
            return RegisterCommand<TParam>(new DelegateCommandAsync<TParam>(execute, canExecute), commandName);
        }
        public IDelegateCommand RegisterCommand(Func<object, Task> execute, Predicate<object> canExecute = null, [CallerMemberName]string commandName = null)
        {
            return RegisterCommand<object>(execute, canExecute, commandName);
        }





    }
}
