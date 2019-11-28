using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.Creator.ViewModels
{
    public class VM_LayerBox:ViewModelBase
    {
        public VM_LayerBox()
        {
            Layers = new ObservableCollection<VM_Layer>();
        }

        public ObservableCollection<VM_Layer> Layers { get { return GetValue<ObservableCollection<VM_Layer>>(); } set { SetValue(value); } }

        public VM_Layer SelectedLayer { get { return GetValue<VM_Layer>(); } set { SetValue(value); } }

        public ICommand Cmd_RemoveLayer
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    var value = p as string;
                    var tx = Layers.FirstOrDefault(t => t.LayerName == value);
                    if (tx != null) Layers.Remove(tx);
                });
            }
        }



    }
}
