using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.Users
{
    public class UserProfileEditModel : IValidatableObject
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CurrentPassword { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public List<string>? Courses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return this.Rules<UserProfileEditModel>(v =>
            {
                //v.RuleFor(x => x.CurrentPassword).NotNull().NotEmpty().WithMessage("Current password is required");
                v.RuleFor(x => x.Password).Equal(p => p.PasswordConfirm).WithMessage("Passwords must be the same as password confirmation");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
