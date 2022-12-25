using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class APIUserCourse
    {
        [Key]
        public long Id { get; set; }
        public string APIUserId { get; set; }
        public APIUser APIUser { get; set; }
        public long CourseId { get; set; }
        public Course Course { get; set; }
        public CourseProgress CourseProgress { get; set; }
    }
}
