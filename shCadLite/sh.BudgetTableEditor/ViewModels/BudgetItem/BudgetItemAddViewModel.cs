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

    class BudgetItemAddViewModel : ViewModelBase
    {
        private BudgetItemViewModel _model;

        public BudgetItemViewModel Model
        {
            get { return _model; ; }
            set { Set(ref _model, value); }
        }


        public BudgetItemAddViewModel()
        {
            Model = new BudgetItemViewModel();

        }

        public RelayCommand SaveBudgetItem
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await Model.SaveAdd();
                    MessageBox.Show(result ? "保存成功" : "保存失败", "提示", MessageBoxButton.OK);
                    if (result)
                    {
                        Messenger.Default.Send<BudgetItemViewModel>(Model, "BudgetItemAddItem");
                        Model = new BudgetItemViewModel();
                    }

                });
            }
        }
    }
}
