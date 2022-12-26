using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class APIUser : IdentityUser
    {
        public APIUser()
        {
            Courses = new HashSet<APIUserCourse>();
            CoursesDone = new HashSet<CourseDone>();
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        public virtual ICollection<APIUserCourse> Courses { get; set; } 
        public virtual ICollection<CourseDone> CoursesDone { get; set; }
    }
}
