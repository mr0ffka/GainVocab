using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Course
{
    public class LearnCourseCheckModel
    {
        public string UserId { get; set; }
        public string UserCoursePublicId { get; set; }
        public string Translation { get; set; }
    }
}
