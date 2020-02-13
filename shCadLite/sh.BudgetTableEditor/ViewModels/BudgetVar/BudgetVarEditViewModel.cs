using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{

    class BudgetVarEditViewModel : ViewModelBase
    {
        public string OldName { get;private set; }

        private BudgetVarViewModel _model;

        public BudgetVarViewModel Model
        {
            get { return _model; ; }
            set { Set(ref _model, value); }
        }


        public BudgetVarEditViewModel()
        {
            Messenger.Default.Register<BudgetVarViewModel>(this, "SetModel", SetModel);
        }

        public RelayCommand SaveBudgetVar
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
                        Messenger.Default.Send<string>(Guid.NewGuid().ToString(), "Refresh");
                    }

                });
            }
        }


        private void SetModel(BudgetVarViewModel model)
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
