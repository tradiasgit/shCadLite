using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sh.Creator.Views.BudgetSheet.ValidationRules
{
    class DoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = this.GetBoundValue(value).ToString();
            if (double.TryParse(stringValue,out var result))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "必须是数字");
            }
        }
    }
}
