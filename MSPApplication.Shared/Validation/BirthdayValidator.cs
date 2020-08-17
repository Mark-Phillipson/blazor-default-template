using System;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared.Validation
{
    public class BirthdayValidator : ValidationAttribute
    {
        public int MinimumAge { get; set; } = 18;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime birthDate;
            if (DateTime.TryParse(value.ToString(), out birthDate))
            {
                if (birthDate < DateTime.Now.AddYears(MinimumAge * -1))
                {
                    return null;
                }
                else
                {
                    return new ValidationResult($"Minimum ages is at least {MinimumAge}", new[] { validationContext.MemberName });
                }
            }
            return new ValidationResult("Invalid birthdate.", new[] { validationContext.MemberName });
        }

    }
}
