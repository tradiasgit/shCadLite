using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
    class BudgetGroupAddViewModel : ViewModelBase
    {
        private BudgetGroupViewModel _model;

        public BudgetGroupViewModel Model
        {
            get { return _model; ; }
            set { Set(ref _model, value); }
        }


        public BudgetGroupAddViewModel()
        {
            Model = new BudgetGroupViewModel();

        }

        public RelayCommand SaveBudgetGroup
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await Model.SaveAdd();
                    MessageBox.Show(result ? "保存成功" : "保存失败", "提示", MessageBoxButton.OK);
                    if (result)
                    {
                        Messenger.Default.Send<BudgetGroupViewModel>(Model, "BudgetGroupAddItem");
                        Model = new BudgetGroupViewModel();
                    }

                });
            }
        }
    }
}
