using System.ComponentModel.DataAnnotations;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.Language
{
    public class AddModel : IValidatableObject
    {
        public string? Name {get; set;}

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<AddModel>(v =>
            {
                v.RuleFor(x => x.Name).NotEmpty().WithMessage("Language name is required");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
