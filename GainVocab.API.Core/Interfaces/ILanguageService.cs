using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.Language;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Interfaces
{
    public interface ILanguageService
    {
        Task<PagedResult<LanguageListModel>> GetList(FilterModel filter, PagerParams pager);
        Task Add(LanguageAddModel entity);
        Language Get(string publicId);
        LanguageListModel GetListModel(string publicId);
        Task Remove(string id);
    }
}
