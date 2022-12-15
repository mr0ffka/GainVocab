using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class APIUserCourse
    {
        public string APIUserId { get; set; }
        public APIUser APIUser { get; set; }
        public long CourseId { get; set; }
        public Course Course { get; set; }
    }
}
