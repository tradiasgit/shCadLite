using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MessageBox = HandyControl.Controls.MessageBox;
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
   
	class BudgetItemViewModel : ViewModelBase
	{
		#region 属性
		private Guid _id;

		public Guid ID
		{
			get { return _id; }
			set { Set(ref _id, value); }
		}

		private string _name;
		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { Set(ref _name, value); }
		}

		private string _expression;
		/// <summary>
		/// 表达式
		/// </summary>
		public string Expression
		{
			get { return _expression; }
			set { Set(ref _expression, value); }
		}

		private double _ratio;
		/// <summary>
		/// 比例
		/// </summary>
		public double Ratio
		{
			get { return _ratio; }
			set { Set(ref _ratio, value); }
		}

		private string _format;
		/// <summary>
		/// 格式化
		/// </summary>
		public string Format
		{
			get { return _format; }
			set { Set(ref _format, value); }
		}

		private string _configurationName;
		/// <summary>
		/// 配置-名称
		/// </summary>
		public string ConfigurationName
		{
			get { return _configurationName; }
			set { Set(ref _configurationName, value); }
		}

		private string _configurationUrl;
		/// <summary>
		/// 配置-URL
		/// </summary>
		public string ConfigurationUrl
		{
			get { return _configurationUrl; }
			set { Set(ref _configurationUrl, value); }
		}

		private double _configurationPrice;
		/// <summary>
		/// 配置-价格
		/// </summary>
		public double ConfigurationPrice
		{
			get { return _configurationPrice; }
			set { Set(ref _configurationPrice, value); }
		}

		private string _groupName;
		/// <summary>
		/// 分组
		/// </summary>
		public string GroupName
		{
			get { return _groupName; }
			set { Set(ref _groupName, value); }
		}
		#endregion


		public BudgetItemViewModel()
		{
		}



		public async Task<bool> SaveAdd()
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			var model = new BudgetItem
			{
				Name = Name,
				Expression = Expression,
				Configuration = new BudgetItemConfiguration { Name = ConfigurationName, Url = ConfigurationUrl, Price = ConfigurationPrice },
				Ratio = Ratio,
				Format = Format
			};
			ID = model.ID;
			btfh.BudgetItemAdd(GroupName, model);
			return await btfh.SaveBudgetTableAsync();
		}

		public async Task<bool> SaveEdit()
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			var model = new BudgetItem
			{
				ID=ID,
				Name = Name,
				Expression = Expression,
				Configuration = new BudgetItemConfiguration { Name = ConfigurationName, Url = ConfigurationUrl, Price = ConfigurationPrice },
				Ratio = Ratio,
				Format = Format
			};
			btfh.BudgetItemEdit(GroupName, model);
			return await btfh.SaveBudgetTableAsync();
		}

		public async Task<bool> SaveRemove()
		{
			var btfh = SimpleIoc.Default.GetInstance<BudgetTableFileHelper>();
			var model = new BudgetItem
			{
				ID = ID,
				Name = Name,
				Expression = Expression,
				Configuration = new BudgetItemConfiguration { Name = ConfigurationName, Url = ConfigurationUrl, Price = ConfigurationPrice },
				Ratio = Ratio,
				Format = Format
			};
			btfh.BudgetItemRemove(GroupName, model);
			return await btfh.SaveBudgetTableAsync();
		}


		public RelayCommand EditBudgetItem
		{
			get
			{
				return new RelayCommand(() =>
				{
					//var win = new BudgetVarEdit();
					//Messenger.Default.Send<BudgetVarViewModel>(new BudgetVarViewModel(Name, Method, Value), "SetModel");
					//win.ShowDialog();
				});
			}
		}

		public RelayCommand RemoveBudgetItem
		{
			get
			{
				return new RelayCommand(async () =>
				{
					var result = await SaveRemove();
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
