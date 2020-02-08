using sh.DesignHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.DesignHub.ViewModels
{
    class VM_ButtonSync : ViewModelBase
    {
        public VM_ButtonSync(RepositoryConfig r)
        {
            Title = "同步到云端";
            repo = r;
        }

        RepositoryConfig repo;


        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Message { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Cmd
        {
            get
            {
                return RegisterCommandAsync(async p =>
                {
                    Title = "正在同步到远程仓库...";
                    IsBusy = true;
                    Message = "正在收集信息...";
                    await Task.Delay(2000);
                    repo.Push();
                    Title = "同步到云端";
                    Message = "同步完成";
                    IsBusy = false;
                });
            }
        }
    }
}
