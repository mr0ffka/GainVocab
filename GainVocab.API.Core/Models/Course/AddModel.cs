using System.ComponentModel.DataAnnotations;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.Course
{
    public class AddModel : IValidatableObject
    {
        public string? Name {get; set;}
        public string? LanguageFrom {get; set;}
        public string? LanguageTo { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<AddModel>(v =>
            {
                v.RuleFor(x => x.Name).NotEmpty().WithMessage("Course name is required");
                v.RuleFor(x => x.LanguageFrom).NotEmpty().WithMessage("Language from is required");
                v.RuleFor(x => x.LanguageTo).NotEmpty().WithMessage("Language to is required");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
