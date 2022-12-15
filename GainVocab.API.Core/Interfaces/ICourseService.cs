using GainVocab.API.Core.Models.Course;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface ICourseService
    {
        Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager);
        List<ItemModel> GetOptionsList();
        List<Course> GetListByPublicId(List<string> courses);
        List<Course> GetListByUser(APIUser user);
        Task Add(AddModel entity);
        Course Get(string publicId);
        Course Get(long id);
        ListItemModel GetListModel(string publicId);
        Task Remove(string id);
    }
}
