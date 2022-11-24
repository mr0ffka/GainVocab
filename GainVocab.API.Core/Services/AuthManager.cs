using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Users;
using GainVocab.API.Data.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace GainVocab.API.Core.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper Mapper;
        private readonly UserManager<APIUser> UserManager;
        private readonly SignInManager<APIUser> SignInManager;
        private readonly IConfiguration Configuration;
        private readonly IEmailService EmailService;
        private readonly ILogger<AuthManager> Logger;

        private string loginProvider = "GainVocabApi";
        private const string refreshToken = "RefreshToken";

        public AuthManager(
            IMapper mapper, 
            UserManager<APIUser> userManager, 
            SignInManager<APIUser> signInManager, 
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<AuthManager> logger)
        {
            Mapper = mapper;
            UserManager = userManager;
            SignInManager = signInManager;
            Configuration = configuration;
            EmailService = emailService;
            Logger = logger;
        }

        public async Task<AuthResponseModel> Login(LoginModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null) return null;

            bool isValidUser = await UserManager.CheckPasswordAsync(user, model.Password);
            var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (!result.Succeeded)
            {
                if (!user.EmailConfirmed)
                {
                    throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedMessage_EmailNotConfirmed);
                }
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedMessage_IncorrectCredentials);
            }
            if (await UserManager.IsLockedOutAsync(user) && isValidUser)
            {
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedMessage_AccountLocked);
            }

            var token = await GenerateToken(user);
            var refreshToken = await CreateRefreshToken(user);
            var rolesUserList = (await UserManager.GetRolesAsync(user)).ToList();

            return new AuthResponseModel
            {
                Token = token,
                UserId = user.Id,
                Roles = rolesUserList,
                RefreshToken = refreshToken,
            };
        }

        public async Task Logout()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task<IEnumerable<IdentityError>> Register(RegisterModel registrationModel)
        {
            var user = Mapper.Map<APIUser>(registrationModel);
            user.UserName = registrationModel.Email;
            user.EmailConfirmed = false;

            var result = await UserManager.CreateAsync(user, registrationModel.Password);

            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user, "User");
            }

            if (!result.Errors.Any())
            {
                var emailVerificationCode = HttpUtility.UrlEncode(await UserManager.GenerateEmailConfirmationTokenAsync(user));
                await EmailService.SendEmailConfirmationEmail(user, emailVerificationCode);
            }

            return result.Errors;
        }

        public async Task<AuthResponseModel> VerifyRefreshToken(AuthResponseModel request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            var user = await UserManager.FindByNameAsync(username);

            if (user == null || user.Id != request.UserId)
            {
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedMessage_IncorrectCredentials);
            }

            var isValidRefreshToken = await UserManager.VerifyUserTokenAsync(user, loginProvider, refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken(user);
                return new AuthResponseModel
                {
                    Token = token,
                    UserId = user.Id,
                    RefreshToken = await CreateRefreshToken(user)
                };
            }

            await UserManager.UpdateSecurityStampAsync(user);
            return null;
        }

        public async Task<AuthResponseModel> OAuthLogin(OAuthLoginModel oauthModel)
        {
            var payload = await VerifyGoogleToken(oauthModel);
            if (payload is null)
            {
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedMessage_InvalidLoginAttempt);
            }
            var info = new UserLoginInfo(oauthModel.Provider, payload.Subject, oauthModel.Provider);

            var user = await UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await UserManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new APIUser { Email = payload.Email, UserName = payload.Email, LastName = payload.FamilyName, FirstName = payload.GivenName };

                    await UserManager.CreateAsync(user);
                    await UserManager.AddLoginAsync(user, info);
                }
                else
                {
                    await UserManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedMessage_InvalidLoginAttempt);

            var token = await GenerateToken(user);
            var refreshToken = await CreateRefreshToken(user);
            var rolesUserList = (await UserManager.GetRolesAsync(user)).ToList();

            return new AuthResponseModel
            {
                Token = token,
                UserId = user.Id,
                Roles = rolesUserList,
                RefreshToken = refreshToken,
            };
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(OAuthLoginModel oauthModel)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { Configuration["OAuth2Settings:Google:ClientId"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(oauthModel.Token, settings);
            return payload;
        }

        public AuthenticationProperties ConfigureExternalAuthProp(string provider, string redirectUrl)
        {
            return SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfo()
        {
            return await SignInManager.GetExternalLoginInfoAsync();

        }

        public async Task<IdentityResult> ConfirmEmailAddress(string userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed();

            return await UserManager.ConfirmEmailAsync(user, code);
        }

        public async Task<IdentityResult> ForgotPassword(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed();

            var forgorPasswordCode = HttpUtility.UrlEncode(await UserManager.GeneratePasswordResetTokenAsync(user));
            await EmailService.SendForgotPasswordEmail(user, forgorPasswordCode);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordModel resetPassword)
        {
            var user = await UserManager.FindByIdAsync(resetPassword.UserId);
            if (user == null)
                return IdentityResult.Failed();

            return await UserManager.ResetPasswordAsync(user, resetPassword.ResetToken, resetPassword.NewPassword);
        }

        private async Task<string> CreateRefreshToken(APIUser user)
        {
            await UserManager.RemoveAuthenticationTokenAsync(user, loginProvider, refreshToken);
            var newRefreshToken = await UserManager.GenerateUserTokenAsync(user, loginProvider, refreshToken);
            var result = await UserManager.SetAuthenticationTokenAsync(user, loginProvider, refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        private async Task<string> GenerateToken(APIUser user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(Configuration["JwtSettings:DurationInMinutes"]));

            var roles = await UserManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await UserManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: Configuration["JwtSettings:Issuer"],
                audience: Configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}