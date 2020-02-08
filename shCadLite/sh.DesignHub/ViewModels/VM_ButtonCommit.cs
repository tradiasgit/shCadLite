using LibGit2Sharp;
using sh.DesignHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sh.DesignHub.ViewModels
{
    class VM_ButtonCommit : ViewModelBase
    {
        public VM_ButtonCommit(RepositoryConfig r)
        {
            repo = r;
            Title = "提交";
            //timer = new System.Timers.Timer(5000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Enabled = true;
            //GC.KeepAlive(timer);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            changes = repo.GetChangeAsync();
            Message = $"收集到{changes.Count()}个变更";
        }

        RepositoryConfig repo;

        System.Timers.Timer timer;
        Patch changes;




        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Message { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand Cmd
        {
            get
            {
                return RegisterCommandAsync(async p =>
                {
                    Title = "正在提交到暂存...";
                    IsBusy = true;
                    Message = "正在收集信息...";
                    await Task.Delay(500);
                    var change = repo.GetChangeAsync();
                    var count = change.Count();
                    Message = $"收集到{count}个变更";
                    await Task.Delay(2000);
                    if (change.Any()) repo.Commit();
                    Title = "提交";
                    Message = $"{count}个变更已提交";
                    IsBusy = false;
                });
            }
        }
    }
}
