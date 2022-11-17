using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Data;
using GainVocab.API.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GainVocab.API.Data.Models;
using Google.Apis.Auth;
using GainVocab.API.Core.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace GainVocab.API.Core.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<APIUser> userManager;
        private readonly SignInManager<APIUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthManager> logger;
        private APIUser user;

        private string loginProvider = "GainVocabApi";
        private const string refreshToken = "RefreshToken";

        public AuthManager(
            IMapper mapper, 
            UserManager<APIUser> userManager, 
            SignInManager<APIUser> signInManager, 
            IConfiguration configuration, 
            ILogger<AuthManager> logger)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<string> CreateRefreshToken()
        {
            await userManager.RemoveAuthenticationTokenAsync(user, loginProvider, refreshToken);
            var newRefreshToken = await userManager.GenerateUserTokenAsync(user, loginProvider, refreshToken);
            var result = await userManager.SetAuthenticationTokenAsync(user, loginProvider, refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        private async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["JwtSettings:DurationInMinutes"]));

            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseModel> Login(LoginModel model)
        {
            user = await userManager.FindByEmailAsync(model.Email);
            bool isValidUser = await userManager.CheckPasswordAsync(user, model.Password);
            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (!result.Succeeded)
            {
                return null;
            }
            if (await userManager.IsLockedOutAsync(user) && isValidUser)
            {
                throw new UnauthorizedAccessException("Locked Account");
            }

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();
            var rolesUserList = (await userManager.GetRolesAsync(user)).ToList();

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
            await signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<IdentityError>> Register(RegisterModel registrationModel)
        {
            user = mapper.Map<APIUser>(registrationModel);
            user.UserName = registrationModel.Email;
            user.IsEmailConfirmed = false;

            var result = await userManager.CreateAsync(user, registrationModel.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }

        public async Task<AuthResponseModel> VerifyRefreshToken(AuthResponseModel request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            user = await userManager.FindByNameAsync(username);

            if (user == null || user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await userManager.VerifyUserTokenAsync(user, loginProvider, refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseModel
                {
                    Token = token,
                    UserId = user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await userManager.UpdateSecurityStampAsync(user);
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

            var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new APIUser { Email = payload.Email, UserName = payload.Email, LastName = payload.FamilyName, FirstName = payload.GivenName };

                    await userManager.CreateAsync(user);
                    await userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await userManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                throw new AuthException("Invalid login attempt");

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();
            var rolesUserList = (await userManager.GetRolesAsync(user)).ToList();

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
                    Audience = new List<string>() { configuration["OAuth2Settings:Google:ClientId"] }
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
            return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfo()
        {
            return await signInManager.GetExternalLoginInfoAsync();

        }
    }
}