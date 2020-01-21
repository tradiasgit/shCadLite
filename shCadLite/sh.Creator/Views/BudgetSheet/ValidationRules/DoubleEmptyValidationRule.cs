using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sh.Creator.Views.BudgetSheet.ValidationRules
{
    class DoubleEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = this.GetBoundValue(value);
            if (stringValue==null)
            {
                return new ValidationResult(false, "不能为空");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
