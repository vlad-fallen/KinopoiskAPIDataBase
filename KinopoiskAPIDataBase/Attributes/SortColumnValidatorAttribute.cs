using System.ComponentModel.DataAnnotations;

namespace KinopoiskAPIDataBase.Attributes
{
    public class SortColumnValidatorAttribute : ValidationAttribute
    {
        public Type EntityType { get; set; }

        public SortColumnValidatorAttribute(Type entityType)
            : base("Value must match an existing column")
        { 
            EntityType = entityType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (EntityType != null)
            {
                var strVal = value as string;
                if (!string.IsNullOrEmpty(strVal) && EntityType.GetProperties().Any(p => p.Name == strVal))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
