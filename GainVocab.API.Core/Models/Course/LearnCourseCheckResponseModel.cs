using GainVocab.API.Core.Models.CourseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Course
{
    public class LearnCourseCheckResponseModel
    {
        public LearnCourseCheckResponseModel()
        {
            Examples = new List<ExampleModel>();
        }

        public bool IsFinished { get; set; }
        public bool IsError { get; set; }
        public int? WordIndexError { get; set; }
        public int PercentProgress { get; set; }
        public bool HasExamples { get; set; }
        public List<ExampleModel> Examples { get; set; }
    }
}
