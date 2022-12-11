using FluentValidation;
using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Users
{
    public class ResetPasswordModel : IValidatableObject
    {
        public string? UserId { get; set; }
        public string? ResetToken { get; set; }
        public string? NewPassword { get; set; }
        public string? NewPasswordConfirm { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<ResetPasswordModel>(v =>
            {
                v.RuleFor(x => x.UserId).NotEmpty().WithMessage("Incorrect user");
                v.RuleFor(x => x.ResetToken).NotEmpty().WithMessage("Incorrect reset password code");
                v.RuleFor(x => x.NewPassword).NotEmpty().Equal(p => p.NewPasswordConfirm).WithMessage("[[[Passwords must be the same]]]");
                v.RuleFor(x => x.NewPassword).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("[[[Incorrect password]]]");

            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
