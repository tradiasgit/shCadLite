﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sh.Creator.Views
{
    /// <summary>
    /// UC_SingleSelectBox.xaml 的交互逻辑
    /// </summary>
    public partial class UC_MultiSelectBox : UserControl
    {
        public UC_MultiSelectBox()
        {
            InitializeComponent();
            var vm = new ViewModels.VM_MultiSelectBox();
            sh.Cad.EventManager.RegisterSelectionListener(vm);
            DataContext = vm;
        }
    }
}
