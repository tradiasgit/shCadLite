using sh.DesignHub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.DesignHub.ViewModels
{
    class VM_Main:ViewModelBase
    {

        public VM_Main()
        {
            SyncTitle = "同步到远程仓库";
            var repos = RepositoryConfig.GetRepositories();
            if (repos != null && repos.Count > 0)
            {
                Repositories = new ObservableCollection<VM_ResourceRepository>(repos.Select(r => new VM_ResourceRepository(r)));
                SelectedRepo = Repositories[0];
            }
        }
        public ObservableCollection<VM_ResourceRepository> Repositories { get { return GetValue<ObservableCollection<VM_ResourceRepository>>(); } set { SetValue(value); } }

        public VM_ResourceRepository SelectedRepo { get { return GetValue<VM_ResourceRepository>(); } set { SetValue(value); } }


        public bool IsSyncing { get { return GetValue<bool>(); } set { SetValue(value); } }

        public string SyncTitle { get { return GetValue<string>(); } set { SetValue(value); } }
        public string SyncMessage { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Cmd_Sync
        {
            get {
                return RegisterCommandAsync(async p =>
                {
                    SyncTitle = "正在同步到远程仓库...";
                    





                    IsSyncing = true;
                    SyncMessage = "正在收集信息...";
                    await Task.Delay(2000);
                    SyncMessage = "正在连接...";
                    await Task.Delay(2000);
                    SyncMessage = "正在上传文件...";
                    await Task.Delay(2000);
                    SyncMessage = "正在更改状态...";
                    await Task.Delay(2000);
                    SyncMessage = "完成";
                    await Task.Delay(1000);
                    SyncTitle = "同步到远程仓库";

                    var c= SelectedRepo.Model.GetChangeAsync();
                    SyncMessage = null;
                    IsSyncing = false;
                });
            }
        }
    }
}
