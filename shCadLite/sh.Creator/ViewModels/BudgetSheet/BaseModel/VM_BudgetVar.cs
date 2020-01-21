using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetVar:ViewModelBase<BudgetVar>
    {
        public string Name 
        {
            get { return Model.Name; }
            set { Model.Name = value;RaisePropertyChanged(); }
        }

        public string Value
        {
            get { return Model.GetValue(); }
            set { Model.SetValue(value); RaisePropertyChanged(); }
        }

        public string Method
        {
            get { return Model.Method; }
            set { Model.Method = value; ; RaisePropertyChanged(); }
        }

    }

    #region MyRegion
    interface IBudgetVar
    {
        string Name { get; set; }

        string Method { get; set; }

        string GetValue();

        void SetValue(string value);
    }

    abstract class BudgetVar : IBudgetVar
    {
        public string Name { get; set; }

        public string Method { get; set; }

        public abstract string GetValue();

        public abstract void SetValue(string value);

        public static List<string> GetMethodList()
        {
            return new List<string> { "Value", "Count", "Length", "Area" };
        }

        public static List<BudgetVar> GetAll(string path = "")
        {
            var list = new List<BudgetVar>();
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support\budgetsheet\budgetvartable.json");
            if (File.Exists(path))
            {
                try
                {
                    list = JsonConvert.DeserializeObject<List<BudgetVar>>(File.ReadAllText(path), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                }
                catch (Exception ex)
                {
                }
            }
            return list;
        }

        public static bool SaveAll(IEnumerable<BudgetVar> list, string path = "")
        {
            var result = false;
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Path.GetDirectoryName(HostApplicationServices.WorkingDatabase.Filename), @"support\budgetsheet\budgetvartable.json");
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, JsonConvert.SerializeObject(list, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    }


    /// <summary>
    /// string
    /// </summary>

    class BudgetVarString : BudgetVar
    {
        public string EcjJsonString { get; set; }
        public override string GetValue()
        {
            return EcjJsonString;
        }

        public override void SetValue(string value)
        {
            EcjJsonString = value;
        }
    }

    /// <summary>
    /// double
    /// </summary>
    class BudgetVarDouble : BudgetVar
    {
        public double Constant { get; set; }

        public override string GetValue()
        {
            return Constant.ToString();
        }

        public override void SetValue(string value)
        {
            if (Double.TryParse(value, out var res))
            {
                Constant = res;
            }
        }
    }
    #endregion
}
