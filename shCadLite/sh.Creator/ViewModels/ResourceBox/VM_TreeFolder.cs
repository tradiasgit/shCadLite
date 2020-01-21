using sh.Creator.Models;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace sh.Creator.ViewModels
{
    public class VM_TreeFolder : VM_TreeItem
    {
        public VM_TreeFolder(DirectoryInfo dir):base(new FileInfo($@"{dir.FullName}\{dir.Name}.json"))
        {
            Text = dir.Name;
            Refresh();
        }

        public void Refresh()
        {
            if (ConfigFile.Exists)
            {
                var text = File.ReadAllText(ConfigFile.FullName);
                Model = JsonConvert.DeserializeObject<ResouceBoxInfo>(text);
                Icon = UI.Common.Icons.CloudDownload ;
                IconBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 140));
                Status = "Default";
            }
            else
            {
                Icon = UI.Common.Icons.FolderClose;
                IconBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 140));
                Status = "Create";
            }
        }

        

        public ObservableCollection<VM_TreeItem> Children { get { return GetValue<ObservableCollection<VM_TreeItem>>(); } set { SetValue(value); } }


        public ResouceBoxInfo Model { get { return GetValue<ResouceBoxInfo>(); } set { SetValue(value); } }

        public bool IsExpanded { get { return GetValue<bool>(); } set { SetValue(value); } }

        public ICommand Cmd_Create
        {
            get
            {
                return RegisterCommand(p =>
                {
                    Model = new ResouceBoxInfo()
                    {
                        ID = Guid.NewGuid(),
                        Name = ConfigFile.Directory.Name,
                    };
                    var text= JsonConvert.SerializeObject(Model);
                    File.WriteAllText(ConfigFile.FullName, text);
                    Refresh();
                });                
            }

        }

    }
}
