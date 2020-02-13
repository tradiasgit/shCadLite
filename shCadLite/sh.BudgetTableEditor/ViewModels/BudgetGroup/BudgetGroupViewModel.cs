using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using sh.BudgetTableEditor.Models;
using sh.BudgetTableEditor.Tools;
using sh.BudgetTableEditor.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace sh.BudgetTableEditor.ViewModels
{
	class BudgetGroupViewModel : ViewModelBase
	{

		private string _name;
		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { Set(ref _name, value); }
		}

		public BudgetGroupViewModel(BudgetGroup  budgetGroup = null)
		{
			if (budgetGroup != null)
			{
				Name = budgetGroup.Name;
			}
		}

		public BudgetGroupViewModel(string name)
		{
			Name = name;
		}


		public async Task<bool> SaveAdd()
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetGroupAdd(new BudgetGroup { Name = Name });
			return await btfh.SaveBudgetTableAsync();
		}

		public async Task<bool> SaveEdit(string oldName)
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetGroupEdit(oldName, new BudgetGroup { Name = Name });
			return await btfh.SaveBudgetTableAsync();
		}

		public async Task<bool> RemoveEdit()
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetGroupRemove(Name);
			return await btfh.SaveBudgetTableAsync();
		}


		public RelayCommand EditBudgetGroup
		{
			get
			{
				return new RelayCommand(() =>
				{
					var win = new BudgetGroupEdit();
					Messenger.Default.Send<BudgetGroupViewModel>(new BudgetGroupViewModel(Name), "SetBudgetGroupEditViewModel");
					win.ShowDialog();
				});
			}
		}

		public RelayCommand RemoveBudgetGroup
		{
			get
			{
				return new RelayCommand(async () =>
				{
					var result = await RemoveEdit();
					MessageBox.Show(result ? "删除成功" : "删除失败", "提示", MessageBoxButton.OK);
					if (result)
					{
						Messenger.Default.Send<string>(Guid.NewGuid().ToString(), "RefreshBudgetGroups");
					}
				});
			}
		}
	}
}
