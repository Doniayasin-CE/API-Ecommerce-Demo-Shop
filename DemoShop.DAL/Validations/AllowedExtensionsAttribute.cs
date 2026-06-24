using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DemoShop.DAL.Validations
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        //string[] _allowedExtensions = { ".jpg",".webp"};
        // 1. Inject allowed extensions dynamically when applying the attribute
        private readonly string[] _allowedExtensions;
        public AllowedExtensionsAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // 2. Safely cast to IFormFile (ignores nulls or other types)
            if (value is IFormFile file)
            {
                // 3. Extract and normalize the extension (e.g., ".JPG" -> ".jpg")
                var extension = Path.GetExtension(file.FileName).ToLower();

                // 4. Validate against the allowed array
                if(!_allowedExtensions.Contains(extension))
                { 
                    var allowed = string.Join(",", _allowedExtensions);
                    return new ValidationResult($"Invalid file type. Allowed:{allowed}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
