using GainVocab.API.Core.Models.CourseData;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface ICourseDataService : IGenericService<CourseData>
    {
        Task<PagedResult<ListItemModel>> GetList(string coursePublicId, FilterModel filter, PagerParams pager);
        Task Add(AddModel entity);
        Task Add(List<AddModel> entities);
        Task Update(string id, UpdateModel model);
        CourseData Get(long id);
        CourseData Get(string publidId);
        CourseData? GetFirstFromCourse(long coursePublicId);
        ItemModel GetItemModel(string publicId);
        Task Remove(string id);
        int GetExamplesCount();
    }
}
