using FluentValidation;
using GainVocab.API.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace GainVocab.API.Core.Models.CourseData
{
    public class UpdateModel : IValidatableObject
    {
        public string Source { get; set; }
        public string Translation { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<UpdateModel>(v =>
            {
                v.RuleFor(x => x.Source).NotEmpty().WithMessage("Source text is required!");
                v.RuleFor(x => x.Translation).NotEmpty().WithMessage("Translation text is required!");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
