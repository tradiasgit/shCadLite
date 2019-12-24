using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetGroup : ViewModelBase<BudgetGroup>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<VM_BudgetItem> _budgetItemss;

        public ObservableCollection<VM_BudgetItem> BudgetItemss
        {
            get { return _budgetItemss; }
            set { _budgetItemss = value; RaisePropertyChanged(); }
        }

        public VM_BudgetGroup(BudgetGroup model)
        {
            Model = model;
            if (model.BudgetItems==null)
            {
                BudgetItemss = new ObservableCollection<VM_BudgetItem>();
            }
            else
            {
                BudgetItemss = new ObservableCollection<VM_BudgetItem>(model.BudgetItems.Select(b => new VM_BudgetItem(b)).ToList());
            }
        }
    }

    public class BudgetGroup
    {
        public string Name { get; set; }

        /// <summary>
        /// 预算
        /// </summary>
        public List<BudgetItem> BudgetItems { get; set; }


        public static List<BudgetGroup> GetAll(string path="")
        {
            var list = new List<BudgetGroup>();
            if(string.IsNullOrEmpty(path))
                path= Path.Combine(Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support\budgetsheet\budgetsheet.json");
            if (File.Exists(path))
            {
                try
                {
                    list = JsonConvert.DeserializeObject<List<BudgetGroup>>(File.ReadAllText(path));
                }
                catch(Exception ex)
                {
                }
            }
            return list;
        }

        public static bool SaveAll(IEnumerable<BudgetGroup> list, string path = "")
        {
            var result = false;
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support\budgetsheet\budgetsheet.json");
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, JsonConvert.SerializeObject(list));
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
