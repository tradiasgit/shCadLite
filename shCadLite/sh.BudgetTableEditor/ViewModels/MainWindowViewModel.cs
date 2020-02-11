using Prism.Commands;
using Prism.Mvvm;
using sh.BudgetTableEditor.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
    class MainWindowViewModel: BindableBase
    {
        private ObservableCollection<LeftMenuItem> _leftMenuItems;

        public ObservableCollection<LeftMenuItem> LeftMenuItems
        {
            get { return _leftMenuItems; }
            set { SetProperty(ref _leftMenuItems, value); }
        }

        private object _subContent;

        public object SubContent
        {
            get { return _subContent; }
            set { SetProperty(ref _subContent, value); }
        }


        private string _contentTitle;

        public string ContentTitle
        {
            get { return _contentTitle; }
            set { SetProperty(ref _contentTitle, value); }
        }




        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private DelegateCommand _clickCommnd;
        public DelegateCommand ClickCommnd =>
            _clickCommnd ?? (_clickCommnd = new DelegateCommand(ExecuteClickCommnd));

        void ExecuteClickCommnd()
        {
            this.Text = "Click Me！";
        }

        public MainWindowViewModel()
        {
            this.Text = "Hello Prism!";
            LeftMenuItems = new ObservableCollection<LeftMenuItem>();
            LeftMenuItems.Add(new LeftMenuItem { Name = "预算",ImagePath= "\xf1ec" });
            LeftMenuItems.Add(new LeftMenuItem { Name = "变量", ImagePath = "\xf0ce" });
        }




        public DelegateCommand<object> SwitchContent
        {
            get
            {
                return new DelegateCommand<object>(p => 
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

    }

    class LeftMenuItem
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }
    }

}
