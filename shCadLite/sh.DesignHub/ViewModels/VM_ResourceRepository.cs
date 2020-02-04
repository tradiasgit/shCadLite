using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using sh.DesignHub.Models;

namespace sh.DesignHub.ViewModels
{
    public class VM_ResourceRepository : ViewModelBase
    {
        public RepositoryConfig Model { get { return GetValue<RepositoryConfig>(); } set { SetValue(value); } }
        public VM_ResourceRepository(RepositoryConfig model)
        {
            Model = model;
            LoadStatus();
            LoadTree();


        }

        private void LoadTree()
        {
            var root = VM_TreeItem.New_VM_TreeItem(new DirectoryInfo(Model.Local)) as VM_TreeFolder;
            ResourceTree = root.Children;
        }

        private void LoadStatus()
        {
            if (!string.IsNullOrWhiteSpace(Model.url) && Directory.Exists(Model.Local) && Directory.Exists($@"{Model.Local}\.git")) Status = "Sync";
            else if (!string.IsNullOrWhiteSpace(Model.url)) Status = "Clone";
            else Status = "Create";
        }

        public string Status { get { return GetValue<string>(); } set { SetValue(value); } }//Create,Clone,Sync

        public ICommand Cmd_Clone
        {
            get
            {
                return RegisterCommandAsync(async p =>
                {
                    SetBusy("正在克隆仓库...");
                    await Model.CloneAsync();
                    SetBusy("正在加载...");
                    await Task.Delay(1000);
                    LoadStatus();
                    ClearBusy();
                });
            }
        }

        public ObservableCollection<VM_TreeItem> ResourceTree { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }




        public ICommand Cmd_Refresh
        {
            get
            {
                return RegisterCommandAsync(async p =>
                {
                    SetBusy("正在加载...");
                    await Task.Delay(200);
                    ClearBusy();
                });
            }
        }


       



        public bool OnProgress(string message)
        {
            BusyMessage = message;
            WriteLine(message);
            return true;
        }






    }
}
