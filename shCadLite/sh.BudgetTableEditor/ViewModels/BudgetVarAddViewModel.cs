using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
    class BudgetVarAddViewModel:ViewModelBase
    {
        private BudgetVarViewModel _model;

        public BudgetVarViewModel Model
        {
            get { return _model;; }
            set { Set(ref _model, value); }
        }


        public BudgetVarAddViewModel()
        {
            Model = new BudgetVarViewModel();

        }

        public  RelayCommand SaveBudgetVar
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await Model.Save();
                    MessageBox.Show(result ? "保存成功" : "保存失败", "提示", MessageBoxButton.OK);
                    if (result)
                    {
                        Messenger.Default.Send<BudgetVarViewModel>(Model, "BudgetVarAddItem");
                        Model = new BudgetVarViewModel();
                    }
                        
                });
            }
        }
    }
}
