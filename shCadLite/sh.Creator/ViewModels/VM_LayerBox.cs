using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using sh.Cad;
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
    public class VM_LayerBox:ViewModelBase,sh.Cad.IEntitySelectionListener
    {

        public ObservableCollection<VM_Layer> Layers { get { return GetValue<ObservableCollection<VM_Layer>>(); } set { SetValue(value); } }


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

        public void OnSelectionChanged(EntitySelection selection)
        {
            IsVisible = false;
            if (selection != null && selection.Count > 1)
            {
                Layers = new ObservableCollection<VM_Layer>(selection.GetEntityies().Select(p=>p.LayerName).Distinct().Select(p=>new VM_Layer(p)).OrderBy(p => p.LayerName));
                IsVisible = true;
            }
        }

        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }
    }
}
