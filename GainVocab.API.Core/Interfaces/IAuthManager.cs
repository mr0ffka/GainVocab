using Microsoft.AspNetCore.Identity;
using GainVocab.API.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(RegistrationModel userModel);
        Task<AuthResponseModel> Login(LoginModel loginDto);
        Task<string> CreateRefreshToken();
        Task<AuthResponseModel> VerifyRefreshToken(AuthResponseModel request);
    }
}
