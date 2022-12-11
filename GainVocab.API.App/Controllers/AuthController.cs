using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Users;
using GainVocab.API.Core.Services;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GainVocab.API.App.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string JWT_TOKEN_COOKIE_NAME = "X-Access-Token";
        private const string REFRESH_TOKEN_COOKIE_NAME = "X-Refresh-Token";

        private readonly IAuthManager AuthManager;
        private readonly UsersService Users;
        private readonly ILogger<AuthController> Logger;

        public AuthController(IAuthManager authManager, UsersService users, ILogger<AuthController> logger)
        {
            AuthManager = authManager;
            Users = users;
            Logger = logger;
        }

        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FrontUserModel>> Register([FromBody] RegisterModel registrationModel)
        {
            Logger.LogInformation($"Registration Attempt for {registrationModel.Email}");

            var errors = await AuthManager.Register(registrationModel);

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            return Ok();

        }

        [HttpGet]
        [Route("verifyemail")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Invalid email confirmation url");
            }

            var status = await AuthManager.ConfirmEmailAddress(userId, code);

            return Ok(status);
        }

        // POST: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FrontUserModel>> Login([FromBody] LoginModel loginModel)
        {
            Logger.LogInformation($"Login Attempt for {loginModel.Email} ");
            var authResponse = await AuthManager.Login(loginModel);
            var user = await Users.GetAsync(authResponse.UserId);
            Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(new
            {
                status = "Success",
                user = new FrontUserModel
                {
                    Id = authResponse.UserId,
                    Email = loginModel.Email,
                    IsAuthenticated = true,
                    IsAdmin = authResponse.Roles.Any(r => r == Enum.GetName(typeof(UserRoles), UserRoles.Administrator)),
                    Roles = authResponse.Roles,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            });

        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {
            await AuthManager.Logout();

            Response.Cookies.Append("X-Access-Token", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(-60) });
            Response.Cookies.Append("X-Refresh-Token", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(-60) });

            return Ok();
        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken()
        {
            Logger.LogInformation($"Refresh Attempt");

            var request = new UserRefreshModel();
            request.RefreshToken = Request.Cookies[REFRESH_TOKEN_COOKIE_NAME];
            request.Token = Request.Cookies[JWT_TOKEN_COOKIE_NAME];

            var authResponse = await AuthManager.VerifyRefreshToken(request);

            Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }
        //[HttpPost("refresh")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult> Refresh()
        //{
        //    var refreshToken = Request.Cookies[REFRESH_TOKEN_COOKIE_NAME];
        //    if (string.IsNullOrEmpty(refreshToken))
        //    {
        //        return Unauthorized();
        //    }

        //    var tokens = await AuthService.Refresh(refreshToken);

        //    if (tokens is null || String.IsNullOrEmpty(tokens.JwtToken) || String.IsNullOrEmpty(tokens.RefreshToken))
        //    {
        //        return BadRequest();
        //    }
        //    Response.Cookies.Append(JWT_TOKEN_COOKIE_NAME, tokens.JwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.AddHours(AuthSettings.JwtExpireHours) });
        //    //Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        //    Response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, tokens.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.AddHours(AuthSettings.RefreshTokenExpireHours) });

        //    return Ok();
        //}

        [HttpPost("googlelogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] OAuthLoginModel model)
        {
            var authResponse = await AuthManager.OAuthLogin(model);
            //Response.Cookies.Append("X-Logged-In", true, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(JWT_TOKEN_COOKIE_NAME, authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(new
            {
                status = "Success",
                user = new FrontUserModel
                {
                    Id = authResponse.UserId,
                    //Email = loginModel.Email,
                    IsAuthenticated = true,
                    IsAdmin = authResponse.Roles.Any(r => r == Enum.GetName(typeof(UserRoles), UserRoles.Administrator)),
                    Roles = authResponse.Roles,
                }
            });
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var status = await AuthManager.ForgotPassword(model.Email);

            return Ok(status);
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            var status = await AuthManager.ResetPassword(resetPassword);

            return Ok(status);
        }

        [HttpGet]
        [Route("me")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FrontUserModel>> GetMe()
        {
            if (User.Identities.Any())
            {
                //Logger.LogInformation($"[api/auth/me] {User.Identity?.Name}");

                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var roleClaims = User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                var uid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
                var roles = new List<string>();
                roleClaims.ForEach(r => roles.Add(r.Value));
                APIUser user = null;

                if (!string.IsNullOrEmpty(uid))
                {
                    user = await Users.GetAsync(uid!);
                }

                var frontUser = new FrontUserModel
                {
                    IsAuthenticated = User.Identity.IsAuthenticated,
                    Email = emailClaim != null ? emailClaim.Value : null,
                    Roles = roles,
                    Id = uid != null ? uid : null,
                    FirstName = user?.FirstName ?? "",
                    LastName = user?.LastName ?? "",
                    IsAdmin = roles.Any(r => r == Enum.GetName(typeof(UserRoles), UserRoles.Administrator))
                };

                return Ok(frontUser);
            }

            return Unauthorized();
        }
    }
}
