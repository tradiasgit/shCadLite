using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace sh.BudgetTableEditor.Tools
{
    class BudgetTableFileHelper
    {
        public string BudgetTableFilePath { get;private set; }

        public BudgetTableFileHelper()
        {
            var args = Environment.GetCommandLineArgs();

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);

            var configurationRoot = builder.Build();

            BudgetTableFilePath = configurationRoot["bedgettablepath"];
        }
    }
}
