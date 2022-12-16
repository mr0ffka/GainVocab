using GainVocab.API.Core.Models.Users;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Models.SupportIssue
{
    public class ListItemModel
    {
        public string? Id { get; set; }
        public string? TypeName { get; set; }
        public bool? IsResolved { get; set; }
        public UserDetailsModel? Reporter { get; set; }
        public IssueEntityListItemModel? IssueEntity { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
