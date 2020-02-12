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
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace sh.Creator.ViewModels.Repository
{
    class VM_RepositoryPalette : CadViewModelBase, IDocumentCollectionListener
    {
        public VM_RepositoryPalette()
        {
            EventManager.RegisterDocumentCollectionListener(this);
        }

        private DocumentEventArgs CurrentDocumentInfo { get { return GetValue<DocumentEventArgs>(); } set { SetValue(value); } }
        public void OnDocumentChanged(DocumentEventArgs e)
        {
            CurrentDocumentInfo = e;
            if (e.DocumentFile != null)
            { Load(); }
            else
            { }
        }

        private void Load()
        {
            try
            {
                Repositories = new ObservableCollection<VM_RepositoryDirectory>();
                Repositories.Add(new VM_RepositoryDirectory("图纸目录", DatabaseDirectory.FullName));
                SelectedRepository = Repositories[0];

                var file = GetFileInfo(AssemblyDirectory, "repositories.json");
                if (!file.Exists) return;
                var array = JsonConvert.DeserializeObject(File.ReadAllText(file.FullName)) as JArray;
                foreach (dynamic jo in array)
                {
                    var name = (string)jo.name;
                    var local = (string)jo.local;
                    Repositories.Add(new VM_RepositoryDirectory(name, local));
                }
            }
            catch (Exception e)
            {
                OnException(e);
            }

        }

    


        public ICommand Cmd_Refresh
        {
            get
            {
                return RegisterCommand(p =>
                {
                    OnDocumentChanged(CurrentDocumentInfo);
                });
            }
        }


        public ObservableCollection<VM_RepositoryDirectory> Repositories { get { return GetValue<ObservableCollection<VM_RepositoryDirectory>>(); } set { SetValue(value); } }

        public VM_RepositoryDirectory SelectedRepository { get { return GetValue<VM_RepositoryDirectory>(); } set { value.LoadTree(); SetValue(value); } }



        public void OnSelectedItemChanged(VM_TreeItem item)
        {
            item?.OnSelect();
        }





    }



}
