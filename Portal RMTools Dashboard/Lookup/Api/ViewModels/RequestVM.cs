using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Api.ViewModels
{

    public class RequestCreated : IValidatableObject
    {
        public string? Type { get; set; }

        public string? Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Type))
            {
                errors.Add(new ValidationResult("Type is required", new[] { nameof(Type) }));
            }
            else if (!Regex.IsMatch(Type, @"^[a-zA-Z\s]+$"))
            {
                errors.Add(new ValidationResult("Column TYPE does not accept characters", new[] { nameof(Type) }));
            }

            if (string.IsNullOrEmpty(Name))
            {
                errors.Add(new ValidationResult("Name is required", new[] { nameof(Name) }));
            }
            else if (!Regex.IsMatch(Name, @"^[a-zA-Z\s]+$"))
            {
                errors.Add(new ValidationResult("Column NAME does not accept characters ", new[] { nameof(Name) }));
            }

            return errors;
        }

    }

    public class ValidationError
    {
        public string Type { get; set; }
        public string Message { get; set; }
    }


}
