using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoShop.DAL.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInMb;
        // 1. Accept dynamic MB limit
        public MaxFileSizeAttribute(int maxFileSizeInMb)
        {
            _maxFileSizeInMb = maxFileSizeInMb;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                // 2. Calculate file size in Megabytes with floating-noint precision
                var fileSizeInMb = file.Length / (1024.0 * 1024.0);
                //3.Compare against the injected maximum threshold
                if (fileSizeInMb > _maxFileSizeInMb)
                    return new ValidationResult($"Maximum allowed file size is {_maxFileSizeInMb} MB");
            }
            return ValidationResult.Success;
        }
    }
}
