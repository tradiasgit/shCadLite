using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HandyControl.Data;
using sh.BudgetTableEditor.Models;
using sh.BudgetTableEditor.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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


		public async Task<bool> Save()
		{
			var btfh= SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			btfh.BudgetVarAdd(new BudgetVar { Name = Name, Method = Method, Value = Value });
			return await btfh.SaveBudgetTableAsync();
		}

	}
}
