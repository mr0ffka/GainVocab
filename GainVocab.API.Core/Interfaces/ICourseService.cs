using GainVocab.API.Core.Models.Course;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GainVocab.API.Core.Interfaces
{
    public interface ICourseService : IGenericService<Course>
    {
        Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager);
        Task<List<ListItemModel>> GetAvailableList(string userId, FilterModel filter);
        Task<List<ActiveListItemModel>> GetActiveList(string userId, FilterModel filter);
        List<ItemModel> GetOptionsList();
        List<Course> GetListByPublicId(List<string> courses);
        List<Course> GetListByUser(APIUser user);
        Task Update(string publicId, string description);
        Task Add(AddModel entity);
        Task AddUser(AddUserToCourseModel entity);
        Course Get(string publicId);
        Course Get(long id);
        Course GetByName(string name);
        ListItemModel GetListModel(string publicId);
        Task Remove(string id);
        Task<LearnCourseModel> GetLearnData(LearnCourseGetModel model);
        Task<LearnCourseCheckResponseModel> CheckLearnWord(LearnCourseCheckModel model);
        Task<LearnCourseNextResponseModel> GetNextWord(LearnCourseNextModel model); 
    }
}
