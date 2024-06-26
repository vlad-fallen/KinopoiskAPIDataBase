﻿using CsvHelper.Configuration.Attributes;
using KinopoiskAPIDataBase.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KinopoiskAPIDataBase.DTO
{
    public class RequestDTO<T> : IValidatableObject
    {
        [DefaultValue(0)]
        public int PageIndex { get; set; } = 0;

        [DefaultValue(10)]
        [Range(1, 100, ErrorMessage = "The value must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        [DefaultValue("Name")]
        public string? SortColumn { get; set; } = "Name";

        [DefaultValue("ASC")]
        public string? SortOrder { get; set; } = "ASC";

        [DefaultValue(null)]
        public string? FilterQuery {  get; set; } = null;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new SortColumnValidatorAttribute(typeof(T));

            var result = validator.GetValidationResult(SortColumn,validationContext);

            return (result != null) ? new[] {result} : new ValidationResult[0];
        }
    }
}
