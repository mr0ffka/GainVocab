using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class CourseData
    {
        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }

        public long CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public ICollection<CourseDataExample>? Examples { get; set; }
        public ICollection<SupportIssue>? Issues { get; set; }
        public ICollection<CourseProgress>? Progresses { get; set; }
    }
}
