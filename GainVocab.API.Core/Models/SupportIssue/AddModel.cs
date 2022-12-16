using System.ComponentModel.DataAnnotations;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.SupportIssue
{
    public class AddModel : IValidatableObject
    {
        public string TypePublicId {get; set;}
        public string ReporterId {get; set;}
        public string? IssueEntityId { get; set; }
        public string IssueMessage { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<AddModel>(v =>
            {
                v.RuleFor(x => x.TypePublicId).NotEmpty().WithMessage("Select issue type!");
                v.RuleFor(x => x.ReporterId).NotEmpty().WithMessage("Issue reporter is required!");
                v.RuleFor(x => x.IssueMessage).NotEmpty().WithMessage("Message is required!");
            })
            .Validate(this, options => options.ThrowOnFailures())
            .Result();
        }
    }
}
