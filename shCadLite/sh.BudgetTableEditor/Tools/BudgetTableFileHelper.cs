using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using sh.BudgetTableEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace sh.BudgetTableEditor.Tools
{
    class BudgetTableFileHelper
    {
        public string BudgetTableFilePath { get; private set; }

        public List<BudgetVar> BudgetVars { get; private set; }

        public List<BudgetGroup> BudgetGroups { get; private set; }



        public BudgetTableFileHelper()
        {
            var args = Environment.GetCommandLineArgs();

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);

            var configurationRoot = builder.Build();

            BudgetTableFilePath = configurationRoot["bedgettablepath"];

            InitBudgetTableAsync();
        }

        #region 预算表
        /// <summary>
        /// 获取预算表
        /// </summary>
        /// <returns></returns>
        public async void InitBudgetTableAsync()
        {
            if (File.Exists(BudgetTableFilePath))
            {
                var jsonStr = await File.ReadAllTextAsync(BudgetTableFilePath);
                var budgetTable = JsonConvert.DeserializeObject<BudgetTable>(jsonStr);
                BudgetVars = budgetTable.BudgetVars;
                BudgetGroups = budgetTable.BudgetGroups;
            }
            if(BudgetVars==null)
                BudgetVars = new List<BudgetVar>();
            if (BudgetGroups == null)
                BudgetGroups = new List<BudgetGroup>();
        }

        /// <summary>
        /// 保存预算
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveBudgetTableAsync()
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(BudgetTableFilePath))
                {
                    var jsonStr = JsonConvert.SerializeObject(new BudgetTable { BudgetVars = BudgetVars, BudgetGroups = BudgetGroups });
                    await File.WriteAllTextAsync(BudgetTableFilePath, jsonStr);
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        #endregion

        #region 变量
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool BudgetVarExists(string name)
        {
            //var bt = await InitBudgetTableAsync();
            //if (bt == null) return true;
            //if (bt.BudgetVars == null) return false;
            return BudgetVars.Exists(v => v.Name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="budgetVar"></param>
        /// <returns></returns>
        public void BudgetVarAdd(BudgetVar budgetVar)
        {
            if (BudgetVars == null)
                BudgetVars = new List<BudgetVar>();
            BudgetVars.Add(budgetVar);
        }
        #endregion



    }
}
