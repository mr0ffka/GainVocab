using FluentValidation;
using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Course
{
    public class AddUserToCourseModel : IValidatableObject
    {
        public string UserId { get; set; }
        public string CoursePublicId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return this.Rules<AddUserToCourseModel>(v =>
            {
                v.RuleFor(x => x.UserId).NotEmpty().WithMessage("Course name is required");
                v.RuleFor(x => x.CoursePublicId).NotEmpty().WithMessage("Language from is required");
            })
           .Validate(this, options => options.ThrowOnFailures())
           .Result();
        }
    }
}
