using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace sh.Creator.Views.BudgetSheet.ValidationRules
{
    class UrlValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = this.GetBoundValue(value) as string;
            string pattern = @"^(http|https)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
            var reg = new Regex(pattern);
            if (reg.IsMatch(stringValue))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Url格式不正确");
            }
        }
    }

   

}
