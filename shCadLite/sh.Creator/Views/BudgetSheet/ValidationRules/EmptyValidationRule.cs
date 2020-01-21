using System;
using System.Globalization;
using System.Windows.Controls;

namespace sh.Creator.Views.BudgetSheet.ValidationRules
{
    class EmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = this.GetBoundValue(value) as string;
            if (String.IsNullOrWhiteSpace(stringValue))
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
