using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sh.Creator.Cad;
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

namespace sh.Creator.ViewModels
{
    public class VM_ResourceBox : CadViewModelBase, IEntitySelectionListener
    {

        public VM_ResourceBox()
        {
            Load();
            IsVisible = true;
        }

        public bool IsVisible { get { return GetValue<bool>(); } set { SetValue(value); } }

        public ObservableCollection<VM_ResourceRepository> Repositories { get { return GetValue<ObservableCollection<VM_ResourceRepository>>(); } set { SetValue(value); } }


        public VM_ResourceRepository SelectedRepo { get { return GetValue<VM_ResourceRepository>(); } set { SetValue(value); } }

        public VM_TreeItem SelectedItem { get { return GetValue<VM_TreeItem>(); } set { SetValue(value); } }


        private void Load()
        {

            Repositories = new ObservableCollection<VM_ResourceRepository>();

            Repositories.Add(new VM_ResourceRepository("图纸目录", DatabaseDirectory));

            SelectedRepo = Repositories[0];
        }

        public void OnSelectionChanged(EntitySelection selection)
        {

            if (selection == null || selection.Count == 0)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }

        public ICommand Cmd_Refresh
        {
            get
            {
                return RegisterCommand(p =>
                {
                    Load();
                });
            }
        }
        public ICommand Cmd_OpenRepoConfig
        {
            get
            {
                return RegisterCommand(p =>
                {
                    Process.Start(sh.ResourceRepository.RepositoryConfig.RepoConfigFile.FullName);
                });
            }
        }


    }
}
