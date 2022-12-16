using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class SupportIssue
    {
        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public bool IsResolved { get; set; }
        public long IssueTypeId { get; set; }
        public string ReporterId { get; set; }
        public string? IssueEntityId { get; set; }
        public string IssueMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual SupportIssueType IssueType { get; set; }
    }
}
