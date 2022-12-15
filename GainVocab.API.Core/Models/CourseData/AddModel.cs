using System.ComponentModel.DataAnnotations;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.CourseData
{
    public class AddModel : IValidatableObject
    {
        public string Source { get; set; }
        public string Translation { get; set; }
        public string CoursePublicId { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<AddModel>(v =>
            {
                v.RuleFor(x => x.Source).NotEmpty().WithMessage("Source text is required!");
                v.RuleFor(x => x.Translation).NotEmpty().WithMessage("Translation text is required!");
                v.RuleFor(x => x.CoursePublicId).NotEmpty().WithMessage("Course is required!");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
