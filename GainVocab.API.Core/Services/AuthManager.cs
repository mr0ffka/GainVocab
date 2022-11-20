using AutoMapper;
using GainVocab.API.Core.Exceptions;
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
        private APIUser User;

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

        public async Task<string> CreateRefreshToken()
        {
            await UserManager.RemoveAuthenticationTokenAsync(User, loginProvider, refreshToken);
            var newRefreshToken = await UserManager.GenerateUserTokenAsync(User, loginProvider, refreshToken);
            var result = await UserManager.SetAuthenticationTokenAsync(User, loginProvider, refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        private async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(Configuration["JwtSettings:DurationInMinutes"]));

            var roles = await UserManager.GetRolesAsync(User);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await UserManager.GetClaimsAsync(User);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, User.Email),
                new Claim("uid", User.Id),
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

        public async Task<AuthResponseModel> Login(LoginModel model)
        {
            User = await UserManager.FindByEmailAsync(model.Email);
            if (User == null) return null;
            if (!User.EmailConfirmed)
            {
                throw new AuthException(AuthException.EmailNotConfirmed);
            }
            bool isValidUser = await UserManager.CheckPasswordAsync(User, model.Password);
            var result = await SignInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, true);

            if (!result.Succeeded)
            {
                return null;
            }
            if (await UserManager.IsLockedOutAsync(User) && isValidUser)
            {
                throw new UnauthorizedAccessException("Locked Account");
            }

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();
            var rolesUserList = (await UserManager.GetRolesAsync(User)).ToList();

            return new AuthResponseModel
            {
                Token = token,
                UserId = User.Id,
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
            User = Mapper.Map<APIUser>(registrationModel);
            User.UserName = registrationModel.Email;
            User.EmailConfirmed = false;

            var result = await UserManager.CreateAsync(User, registrationModel.Password);

            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(User, "User");
            }

            if (!result.Errors.Any())
                await EmailService.SendEmailConfirmation(User);

            return result.Errors;
        }

        public async Task<AuthResponseModel> VerifyRefreshToken(AuthResponseModel request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            User = await UserManager.FindByNameAsync(username);

            if (User == null || User.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await UserManager.VerifyUserTokenAsync(User, loginProvider, refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseModel
                {
                    Token = token,
                    UserId = User.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await UserManager.UpdateSecurityStampAsync(User);
            return null;
        }

        public async Task<AuthResponseModel> OAuthLogin(OAuthLoginModel oauthModel)
        {
            var payload = await VerifyGoogleToken(oauthModel);
            if (payload is null)
            {
                throw new AuthException("Invalid Login Attempt");
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
                throw new AuthException("Invalid login attempt");

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();
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
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { Configuration["OAuth2Settings:Google:ClientId"] }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(oauthModel.Token, settings);
                return payload;
            }
            catch (Exception ex)
            {
                return null;
            }
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
    }
}