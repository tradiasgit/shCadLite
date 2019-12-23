using sh.Cad;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels.SingleSelectBox
{
    public class VM_EntityPrefab:ViewModelBase<sh.Cad.EntityInfo>
    {
        public string Key { get; set; }

        public ICommand Cmd_Brush
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Model?.BrushImplied();
                });
            }
        }
        
        public ICommand Cmd_Draw
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Model?.Draw();
                });
            }
        }

    }
}
