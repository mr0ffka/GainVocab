using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface ILanguageService : IGenericService<Language>
    {
        Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager);
        List<ItemModel> GetOptionsList();
        Task Add(AddModel entity);
        Language Get(string publicId);
        ListItemModel GetListModel(string publicId);
        Task Remove(string id);
    }
}
