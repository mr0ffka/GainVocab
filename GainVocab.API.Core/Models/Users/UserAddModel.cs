﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.Users
{
    public class UserAddModel : IValidatableObject
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PasswordConfirm { get; set; }
        public bool? EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
        public List<string>? Courses { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<UserAddModel>(v =>
            {
                v.RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
                v.RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
                v.RuleFor(x => x.Password).NotEmpty().Equal(p => p.PasswordConfirm).WithMessage("Passwords must be the same");
                v.RuleFor(x => x.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("Password must have at least 8 characters including special characters and numbers!");
                v.RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
                v.RuleFor(x => x.Email).EmailAddress().WithMessage("Incorrect email address");
                v.RuleFor(x => x.Roles).NotEmpty().WithMessage("User role is required");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
