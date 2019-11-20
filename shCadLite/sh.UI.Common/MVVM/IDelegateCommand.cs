using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.UI.Common.MVVM
{
    public interface IDelegateCommand:ICommand
    {
        void RaiseCanExecuteChanged();

        IViewModelBase Parent { get; set; }

        string Name { get; set; }

     
    }
}
