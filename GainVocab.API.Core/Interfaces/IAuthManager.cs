using Microsoft.AspNetCore.Identity;
using GainVocab.API.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace GainVocab.API.Core.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(RegisterModel userModel);
        Task<AuthResponseModel> Login(LoginModel model);
        Task Logout();
        Task<string> CreateRefreshToken();
        Task<AuthResponseModel> VerifyRefreshToken(AuthResponseModel request);
        Task<AuthResponseModel> OAuthLogin(OAuthLoginModel oauthModel);
    }
}
