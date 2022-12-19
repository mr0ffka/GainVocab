using FluentValidation;
using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.CourseData
{
    public class ExampleAddModel : IValidatableObject
    {
        public string Source { get; set; }
        public string Translation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return this.Rules<ExampleAddModel>(v =>
            {
                v.RuleFor(x => x.Source).NotEmpty().WithMessage("Example source text is required!");
                v.RuleFor(x => x.Translation).NotEmpty().WithMessage("Example translation text is required!");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
