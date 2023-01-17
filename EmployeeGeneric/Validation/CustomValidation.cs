using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeGeneric.Validation
{
    public class CustomValidation
    {
    }
    public class DenyHtmlInputAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var tagWithoutClosingRegex = new Regex(@"<[^>]+>");

            var hasTags = tagWithoutClosingRegex.IsMatch(value.ToString());

            if (!hasTags)
                return ValidationResult.Success;

            return new ValidationResult(String.Format("{0} HTML tags not allowed", validationContext.DisplayName));
        }
    }
    public class EmailInputAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(Convert.ToString(value)))
                return ValidationResult.Success;



            var tagWithoutClosingRegex = new Regex(@"\A(?i)(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?-i)[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");



            var hasTags = tagWithoutClosingRegex.IsMatch(value.ToString());



            if (hasTags)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(String.Format("The {0} is not a valid e-mail address.", value));
            }
        }
    }
        public class PhoneInputAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                    return ValidationResult.Success;

                var tagWithoutClosingRegex = new Regex(@"^\d{3}-\d{3}-\d{4}$");

                var hasTags = tagWithoutClosingRegex.IsMatch(value.ToString());

                if (hasTags)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(String.Format("The {0} is Not a valid Phone number. Phone Number must have a format as 000-000-0000.", value));
                }
            }
        }


        public class PasswordInputAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                    return ValidationResult.Success;

                var tagWithoutClosingRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d.*)(?=.*[\W_]+)[a-zA-Z0-9\S]{8,}");

                var hasTags = tagWithoutClosingRegex.IsMatch(value.ToString());

                if (hasTags)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Password must be 8 Characters long and must contain at least one upper case character, one lowercase character, one number and one special character.");
                }
            }
        }
        public class NameInputAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                    return ValidationResult.Success;

                var tagWithoutClosingRegex = new Regex(@"^[a-zA-Z]+(\s[a-zA-Z]+)?$");

                var hasTags = tagWithoutClosingRegex.IsMatch(value.ToString());

                if (hasTags)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(String.Format("{0} : Name should not contain numbers and special characters.", value));
                }
            }
        }
   
}

