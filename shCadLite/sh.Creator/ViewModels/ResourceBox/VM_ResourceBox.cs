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

namespace sh.Creator.ViewModels
{
    public class VM_ResourceBox : ViewModelBase, IEntitySelectionListener
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
            var repos = sh.ResourceRepository.RepositoryConfig.GetRepositories();
            if (repos != null && repos.Count > 0)
            {
                Repositories = new ObservableCollection<VM_ResourceRepository>(repos.Select(r => new VM_ResourceRepository(r)));
                SelectedRepo = Repositories[0];
            }
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
                return CommandFactory.RegisterCommand(p =>
                {
                    Load();
                });
            }
        }
        public ICommand Cmd_OpenRepoConfig
        {
            get
            {
                return CommandFactory.RegisterCommand(p =>
                {
                    Process.Start(sh.ResourceRepository.RepositoryConfig.RepoConfigFile.FullName);
                });
            }
        }


    }
}
