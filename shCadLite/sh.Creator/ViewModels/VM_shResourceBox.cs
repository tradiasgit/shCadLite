using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sh.Cad;
using System.Collections.ObjectModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Xml;

namespace sh.Creator.ViewModels
{
    public class VM_shResourceBox : ViewModelBase
    {

        private static VM_shResourceBox _Instance;
        public static VM_shResourceBox Instance
        {
            get
            {
                if (_Instance == null) _Instance = new VM_shResourceBox();
                return _Instance;
            }
        }



        private VM_shResourceBox()
        {
            LoadCadParts();
            LoadFields();
            LoadBrush();
        }



        private void LoadCadParts()
        {
            var db_source = HostApplicationServices.WorkingDatabase;
            var dir = new FileInfo(db_source.OriginalFileName).Directory;
            dir = new DirectoryInfo($@"{dir.FullName}\support\dwg");
            if (dir.Exists)
            {
                CadParts = new ObservableCollection<VM_CadPart>(dir.GetFiles().Where(f => f.Extension.ToLower() == ".dwg" && !f.Name.ToLower().EndsWith("_recover.dwg")).Select(f => new VM_CadPart(f)));
            }
        }
        private void LoadFields()
        {
            var db_source = HostApplicationServices.WorkingDatabase;
            var dir = new FileInfo(db_source.OriginalFileName).Directory;
            dir = new DirectoryInfo($@"{dir.FullName}\support\field");
            if (dir.Exists)
            {
                Fields = new ObservableCollection<VM_Field>(dir.GetFiles().Where(f => f.Extension.ToLower() == ".xml").Select(f => new VM_Field(f)));
            }
        }

        private void LoadBrush()
        {
            var db_source = HostApplicationServices.WorkingDatabase;
            var dir = new FileInfo(db_source.OriginalFileName).Directory;
            dir = new DirectoryInfo($@"{dir.FullName}\support\brush");
            if (dir.Exists)
            {
                Brushes = new ObservableCollection<VM_Brush>(dir.GetFiles().Where(f => f.Extension.ToLower() == ".xml").Select(f => new VM_Brush(f)));
            }
        }

        public ObservableCollection<VM_CadPart> CadParts { get { return GetValue<ObservableCollection<VM_CadPart>>(); } set { SetValue(value); } }



        public ObservableCollection<VM_Field> Fields { get { return GetValue<ObservableCollection<VM_Field>>(); } set { SetValue(value); } }

        public ObservableCollection<VM_Brush> Brushes { get { return GetValue<ObservableCollection<VM_Brush>>(); } set { SetValue(value); } }
        public ICommand Cmd_Refresh
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    LoadCadParts();
                    LoadFields(); LoadBrush();
                });
            }
        }

    }
}
