using GainVocab.API.Core.Models.SupportIssue;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface ISupportIssueTypeService : IGenericService<SupportIssueType>
    {
        SupportIssueType Get(string publicId);
        SupportIssueType Get(long id);
        //Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager);
        List<TypeItemModel> GetOptionsList();
        //Language Get(string publicId);
        //ListItemModel GetListModel(string publicId);
        //Task Remove(string id);
    }
}
