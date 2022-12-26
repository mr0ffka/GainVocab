using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class CourseProgress
    {
        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public int PercentProgress { get; set; }
        public int AmountOfErrors { get; set; }
        public long UserCourseId { get; set; }
        public APIUserCourse UserCourse { get; set; }
        public long? CurrentCourseDataId { get; set; }
        public CourseData? CurrentCourseData { get; set; }
        public virtual ICollection<CourseProgressDataDone>? DataDone { get; set; }

    }
}
