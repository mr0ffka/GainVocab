using GainVocab.API.Data.Models;
using GainVocab.API.Core.Models.CourseProgress;

namespace GainVocab.API.Core.Interfaces
{
    public interface ICourseProgressService : IGenericService<CourseProgress>
    {
        Task<CourseProgress> Add(AddModel entity);
        Task Remove(long userCourseId);
    }
}
