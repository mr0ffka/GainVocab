using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.Users;
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
    public interface IUsersService : IGenericService<APIUser>
    {
        Task<PagedResult<APIUserModel>> GetList(FilterModel filter, PagerParams pager);
        Task<IdentityResult> Add(UserAddModel newUser);
        APIUser Get(string id);
        Task<APIUserModel> GetUserModel(string id);
        List<string> GetRoleOptionsList();
        List<UserOptionModel> GetUserOptionsList();
        Task<UserDetailsModel> GetDetails(string id);
        Task Remove(string id);
        Task Update(string id, UserEditModel model);
        Task UpdateMe(string id, UserProfileEditModel model);
    }
}
