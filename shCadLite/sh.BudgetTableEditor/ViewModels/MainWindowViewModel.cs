using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using sh.BudgetTableEditor.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
    class MainWindowViewModel: ViewModelBase
    {
        private readonly Tools.BudgetTableFileHelper _budgetTableFileHelper;

        private ObservableCollection<LeftMenuItem> _leftMenuItems;

        public ObservableCollection<LeftMenuItem> LeftMenuItems
        {
            get { return _leftMenuItems; }
            set { Set(ref _leftMenuItems, value); }
        }

        private object _subContent;

        public object SubContent
        {
            get { return _subContent; }
            set { Set(ref _subContent, value); }
        }


        private string _contentTitle;

        public string ContentTitle
        {
            get { return _contentTitle; }
            set { Set(ref _contentTitle, value); }
        }




        private string _text;
        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        private RelayCommand _clickCommnd;
        public RelayCommand ClickCommnd =>
            _clickCommnd ?? (_clickCommnd = new RelayCommand(ExecuteClickCommnd));

        void ExecuteClickCommnd()
        {
            this.Text = "Click Me！";
        }

        public MainWindowViewModel(Tools.BudgetTableFileHelper budgetTableFileHelper)
        {
            this.Text = "Hello Prism!";
            LeftMenuItems = new ObservableCollection<LeftMenuItem>();
            LeftMenuItems.Add(new LeftMenuItem { Name = "预算",ImagePath= "\xf1ec" });
            LeftMenuItems.Add(new LeftMenuItem { Name = "变量", ImagePath = "\xf0ce" });

            _budgetTableFileHelper = budgetTableFileHelper;
        }




        public RelayCommand<object> SwitchContent
        {
            get
            {
                return new RelayCommand<object>(p => 
                {
                    if(p is LeftMenuItem lmi)
                    {
                        switch (lmi.Name)
                        {
                            case "预算":
                                ContentTitle = "预算表";
                                SubContent = new BudgetItemTable();
                                break;
                            case "变量":
                                ContentTitle = "变量表";
                                SubContent = new BudgetVarTable();
                                break;
                        }

                        //MessageBox.Show(lmi.Name, "标题", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    }
                });
            }
        }

        public RelayCommand<object> LoadFileBudget
        {
            get
            {
                return new RelayCommand<object>(async p => 
                {
                    await Task.Run(() =>
                    {
                        Thread.Sleep(10000);
                        MessageBox.Show("degndai", "标题", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        
                    });
                });
            }
        }

    }

    class LeftMenuItem
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }
    }

}
