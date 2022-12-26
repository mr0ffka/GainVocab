using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class CourseProgressDataDone
    {
        public long CourseProgressId { get; set; }
        public CourseProgress CourseProgress { get; set; }
        public long CourseDataId { get; set; }
        public CourseData CourseData { get; set; }
    }
}
