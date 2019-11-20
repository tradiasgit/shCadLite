using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.UI.Common.MVVM
{
    public class ViewHelper
    {
        public virtual void ShowMessage(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public virtual bool Confirm(string message)
        {
            var result = System.Windows.MessageBox.Show(message, "", System.Windows.MessageBoxButton.YesNo);
            return result == System.Windows.MessageBoxResult.Yes;
        }
    }
}
