using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.Language
{
    public class LanguageAddModel : IValidatableObject
    {
        public string? Name {get; set;}

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<LanguageAddModel>(v =>
            {
                v.RuleFor(x => x.Name).NotEmpty().WithMessage("Language name is required");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
