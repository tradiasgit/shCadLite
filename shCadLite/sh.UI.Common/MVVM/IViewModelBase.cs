using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.UI.Common.MVVM
{
    public interface IViewModelBase: INotifyPropertyChanged
    {
        string WorkingDirectory { get; set; }

        string AuthorizationFileName { get; set; }
    }

    
}
