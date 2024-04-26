using System.ComponentModel.DataAnnotations;

namespace KinopoiskAPIDataBase.Attributes
{
    public class SortOrderValidatorAttribute : ValidationAttribute
    {
        public string[] AllowedValues { get; set; } = new[] { "ASC", "DESC" };

        public SortOrderValidatorAttribute()
            : base("Value must be one of the following: {0}") { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var strVal = value as string;

            if (!string.IsNullOrEmpty(strVal) && AllowedValues.Contains(strVal))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(string.Join(",", AllowedValues)));
        }
    }
}
