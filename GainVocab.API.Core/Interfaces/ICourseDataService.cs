using GainVocab.API.Core.Models.CourseData;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface ICourseDataService : IGenericService<CourseData>
    {
        Task<PagedResult<ListItemModel>> GetList(string coursePublicId, FilterModel filter, PagerParams pager);
        Task Add(AddModel entity);
        Task Update(string id, UpdateModel model);
        CourseData Get(string publidId);
        ItemModel GetItemModel(string publicId);
        Task Remove(string id);
    }
}
