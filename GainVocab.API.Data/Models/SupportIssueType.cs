using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class SupportIssueType
    {
        public SupportIssueType()
        {
            Issues = new HashSet<SupportIssue>();
        }

        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SupportIssue> Issues { get; set; }
    }
}
