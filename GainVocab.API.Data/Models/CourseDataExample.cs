using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class CourseDataExample
    {
        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }

        public long CourseDataId { get; set; }
        public virtual CourseData? CourseData { get; set; }

    }
}
