using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeGeneric.Helper
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var mesage = string.Join(Environment.NewLine, context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
                context.Result = new BadRequestObjectResult(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = false, message = mesage }));
            }
            return base.OnActionExecutionAsync(context, next);
        }

        public sealed class MaxFileSize : ValidationAttribute
        {
            public int MaxSize { get; set; }



            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    if (file.Length > MaxSize)
                    {
                        return new ValidationResult($"'{file.FileName}' should be less then {MaxSize / 1024 / 1024}MB.");
                    }
                }

                return ValidationResult.Success;
            }

        }
        public sealed class ValidFileType : ValidationAttribute
        {
            public string[] Extensions { get; set; }



            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!Extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult($"'{file.FileName}' is not a valid file type. Only {String.Join(", ", Extensions)} file types are allowed.");
                    }
                }

                return ValidationResult.Success;
            }
        }
    }
}
