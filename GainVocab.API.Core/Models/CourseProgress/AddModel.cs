using GainVocab.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.CourseProgress
{
    public class AddModel
    {
        public AddModel(APIUserCourse? userCourse)
        {
            UserCourse = userCourse;
        }

        public int PercentProgress { get; set; } = 0;
        public int AmountOfErrors { get; set; } = 0;
        public APIUserCourse? UserCourse { get; set; }
    }
}
