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
    public class LoginModel : IValidatableObject
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<LoginModel>(v =>
            {
                v.RuleFor(x => x.Email).EmailAddress().WithMessage("[[[Email is required]]]");
                v.RuleFor(x => x.Password).NotEmpty().WithMessage("[[[Password is required]]]");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
