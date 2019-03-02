using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Web.Models.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class RemoveScriptAttribute : ValidationAttribute
    {
        public const string DefaultRegexPattern = @"\<((?=(?!\b(a|b|i|p|u|br|hr)\b))(?=(?!\/\b(a|b|i|p|u)\b))).*?\>";

        public string RegexPattern { get; }

        public RemoveScriptAttribute(string regexPattern = null)
        {
            RegexPattern = regexPattern ?? DefaultRegexPattern;
        }

        protected override ValidationResult IsValid(object value, ValidationContext ctx)
        {
            var valueStr = value as string;
            if (valueStr != null)
            {
                var newVal = Regex.Replace(valueStr, RegexPattern, "", RegexOptions.IgnoreCase, new TimeSpan(0, 0, 0, 0, 250));

                if (newVal != valueStr)
                {
                    var prop = ctx.ObjectType.GetProperty(ctx.MemberName);
                    prop.SetValue(ctx.ObjectInstance, newVal);
                }
            }

            return null;
        }
    }
}