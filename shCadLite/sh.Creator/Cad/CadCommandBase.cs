using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.Cad
{
    abstract class CadCommandBase:ICommand
    {

        public event EventHandler CanExecuteChanged;
        protected void OnCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
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
                    if (CanExecute(parameter)) { ExecuteBody(parameter); ed.Regen(); }
                }
                catch (Exception ex)
                {
                    ed.WriteMessage($"{Environment.NewLine}【异常】{ex.Message}{Environment.NewLine}");
                }
            }
        }

        protected abstract void ExecuteBody(object parameter);



        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

    }
}
