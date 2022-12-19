using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.SupportIssue;
using GainVocab.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Interfaces
{
    public interface ISupportIssueService : IGenericService<SupportIssue>
    {
        Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager);
        Task Add(AddModel entity);
        SupportIssue Get(string publicId);
        Task Remove(string id);
        Task Resolve(string publicId);
    }
}
