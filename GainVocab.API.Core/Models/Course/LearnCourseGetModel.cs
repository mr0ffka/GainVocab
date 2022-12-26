using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Course
{
    public class LearnCourseGetModel
    {
        public string UserId { get; set; }
        public string CoursePublicId { get; set; }
    }
}
