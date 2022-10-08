using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GainVocab.Core.Extensions;

namespace GainVocab.Core.Models.Users
{
    public class RegistrationModel : APIUserModel, IValidatableObject
    {
        public string? PasswordConfirmation { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<RegistrationModel>(v =>
            {
                v.RuleFor(x => x.FirstName).NotEmpty().WithMessage("[[[First Name is required]]]");
                v.RuleFor(x => x.LastName).NotEmpty().WithMessage("[[[Last Name is required]]]");
                v.RuleFor(x => x.Password).NotEmpty().Equal(p => p.PasswordConfirmation).WithMessage("[[[Passwords must be the same]]]");
                v.RuleFor(x => x.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("[[[Incorrect password]]]");
                v.RuleFor(x => x.Email).NotEmpty().WithMessage("[[[Email is required]]]");
                v.RuleFor(x => x.Email).EmailAddress().WithMessage("[[[Incorrect email address]]]");
            })
            .Validate(this)
            .Result();
        }
    }
}
