using GainVocab.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Interfaces
{
    public interface IUserCourseService : IGenericService<APIUserCourse>
    {
        APIUserCourse Get(string id);
    }
}
