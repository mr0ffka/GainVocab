using GainVocab.API.Core.Models.CourseData;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface ICourseDataService
    {
        Task<PagedResult<ListItemModel>> GetList(string coursePublicId, FilterModel filter, PagerParams pager);
        Task Add(AddModel entity);
        Task Update(string id, UpdateModel model);
        ItemModel GetItemModel(string publicId);
        //Course Get(string publicId);
        //Course Get(long id);
        Task Remove(string id);
    }
}
