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
    public class UserEditModel : IValidatableObject
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PasswordConfirm { get; set; }
        public List<string> Roles { get; set; }
        public List<string>? Courses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return this.Rules<UserEditModel>(v =>
            {
                v.RuleFor(x => x.Password).Equal(p => p.PasswordConfirm).WithMessage("Passwords must be the same");
                v.RuleFor(x => x.Roles).NotEmpty().WithMessage("At least one role is required!");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
