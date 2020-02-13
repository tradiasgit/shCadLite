using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MessageBox= HandyControl.Controls.MessageBox;
using HandyControl.Data;
using sh.BudgetTableEditor.Models;
using sh.BudgetTableEditor.Tools;
using sh.BudgetTableEditor.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sh.BudgetTableEditor.ViewModels
{
	class BudgetVarViewModel : ViewModelBase
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

		private string _method;
		/// <summary>
		/// 方法
		/// </summary>
		public string Method
		{
			get { return _method; }
			set { Set(ref _method, value); }
		}

		private string _value;
		/// <summary>
		/// 值
		/// </summary>
		public string Value
		{
			get { return _value; }
			set { Set(ref _value, value); }
		}

		public BudgetVarViewModel(BudgetVar budgetVar = null)
		{
			if (budgetVar != null)
			{
				Name = budgetVar.Name;
				Method = budgetVar.Method;
				Value = budgetVar.Value;
			}
		}

		public BudgetVarViewModel(string name, string method, string value)
		{
			Name = name;
			Method = method;
			Value = value;
		}


		public async Task<bool> SaveAdd()
		{
			var btfh= SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetVarAdd(new BudgetVar { Name = Name, Method = Method, Value = Value });
			return await btfh.SaveBudgetTableAsync();
		}

		public async Task<bool> SaveEdit(string oldName)
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetVarEdit(oldName,new BudgetVar { Name = Name, Method = Method, Value = Value });
			return await btfh.SaveBudgetTableAsync();
		}

		public async Task<bool> RemoveEdit()
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetVarRemove(Name);
			return await btfh.SaveBudgetTableAsync();
		}


		public RelayCommand EditBudgetVar
		{
			get
			{
				return new RelayCommand( () =>
				{
					var win = new BudgetVarEdit();
					Messenger.Default.Send<BudgetVarViewModel>(new BudgetVarViewModel(Name,Method,Value), "SetModel");
					win.ShowDialog();
				});
			}
		}

		public RelayCommand RemoveBudgetVar
		{
			get
			{
				return new RelayCommand(async() =>
				{
					var result = await RemoveEdit();
					MessageBox.Show(result ? "删除成功" : "删除失败", "提示", MessageBoxButton.OK);
					if (result)
					{
						Messenger.Default.Send<string>(Guid.NewGuid().ToString(), "Refresh");
					}
				});
			}
		}
	}
}
