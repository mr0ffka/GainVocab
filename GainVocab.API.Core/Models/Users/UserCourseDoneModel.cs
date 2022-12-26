using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Users
{
    public class UserCourseDoneModel
    {
        public string CourseName { get; set; }
        public int AmountOfErrors { get; set; }
    }
}
