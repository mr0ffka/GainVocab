using GainVocab.API.Core.Models.CourseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Course
{
    public class LearnCourseModel
    {
        public string CurrentDataPublicId { get; set; }
        public string UserCoursePublicId { get; set; }
        public bool IsFinished { get; set; }
        public string? Name { get; set; }
        public string? LanguageFrom { get; set; }
        public string? LanguageTo { get; set; }
        public string Source { get; set; }
        public int PercentProgress { get; set; }
    }
}
