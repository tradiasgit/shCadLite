using sh.UI.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sh.Creator.ViewModels.BudgetSheet
{
    class VM_BudgetItemConfiguration:ViewModelBase<BudgetItemConfiguration>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value;RaisePropertyChanged(); }
        }

        public string Url
        {
            get { return Model.Url; }
            set { Model.Url = value; RaisePropertyChanged(); }
        }

        public double Price
        {
            get { return Model.Price; }
            set { Model.Price = value; RaisePropertyChanged(); }
        }

    }

    /// <summary>
    /// 预算配置-商品
    /// </summary>
    public class BudgetItemConfiguration
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public double Price { get; set; }

    }
}
