using Microsoft.AspNetCore.Identity;
using GainVocab.API.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(RegisterModel userModel);
        Task<AuthResponseModel> Login(LoginModel model);
        Task Logout();
        Task<AuthResponseModel> VerifyRefreshToken(UserRefreshModel request);
        Task<bool> ConfirmEmailAddress(string userId, string code);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordModel resetPasswordModel);
    }
}
