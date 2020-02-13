using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
    class BudgetGroupEditViewModel : ViewModelBase
    {
        public string OldName { get; private set; }

        private BudgetGroupViewModel _model;

        public BudgetGroupViewModel Model
        {
            get { return _model; ; }
            set { Set(ref _model, value); }
        }


        public BudgetGroupEditViewModel()
        {
            Messenger.Default.Register<BudgetGroupViewModel>(this, "SetBudgetGroupEditViewModel", SetModel);
        }

        public RelayCommand SaveBudgetGroup
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await Model.SaveEdit(OldName);
                    MessageBox.Show(result ? "编辑成功" : "编辑失败", "提示", MessageBoxButton.OK);
                    if (result)
                    {
                        OldName = Model.Name;
                        Messenger.Default.Send<string>(Guid.NewGuid().ToString(), "RefreshBudgetGroups");
                    }

                });
            }
        }


        private void SetModel(BudgetGroupViewModel model)
        {
            Model = model;
            OldName = model.Name;
        }


        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }
    }
}
