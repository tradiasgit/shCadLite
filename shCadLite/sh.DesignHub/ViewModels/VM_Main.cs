using sh.DesignHub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.DesignHub.ViewModels
{
    class VM_Main:ViewModelBase
    {

        public VM_Main()
        {
            var repos = RepositoryConfig.GetRepositories();
            if (repos != null && repos.Count > 0)
            {
                Repositories = new ObservableCollection<VM_ResourceRepository>(repos.Select(r => new VM_ResourceRepository(r)));
                SelectedRepo = Repositories[0];
            }
        }
        public ObservableCollection<VM_ResourceRepository> Repositories { get { return GetValue<ObservableCollection<VM_ResourceRepository>>(); } set { SetValue(value); } }

        public VM_ResourceRepository SelectedRepo { get { return GetValue<VM_ResourceRepository>(); } set { SetValue(value); } }
    }
}
