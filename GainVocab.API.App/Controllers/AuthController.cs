using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Users;
using GainVocab.API.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;

namespace GainVocab.API.App.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthManager authManager, ILogger<AuthController> logger)
        {
            this.authManager = authManager;
            this.logger = logger;
        }

        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FrontUserModel>> Register([FromBody] RegisterModel registrationModel)
        {
            logger.LogInformation($"Registration Attempt for {registrationModel.Email}");
            var errors = await authManager.Register(registrationModel);

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            var authResponse = await authManager.Login(new LoginModel
            {
                Email = registrationModel.Email,
                Password = registrationModel.Password,
                RememberMe = false
            });

            Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(new
            {
                status = "Success",
                user = new FrontUserModel
                {
                    Id = authResponse.UserId,
                    Email = registrationModel.Email,
                    IsAuthenticated = true,
                    IsAdmin = authResponse.Roles.Any(r => r == Enum.GetName(typeof(UserRoles), UserRoles.Administrator)),
                    Roles = authResponse.Roles,
                }
            });

        }

        // POST: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FrontUserModel>> Login([FromBody] LoginModel loginModel)
        {
            logger.LogInformation($"Login Attempt for {loginModel.Email} ");
            var authResponse = await authManager.Login(loginModel);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            //Response.Cookies.Append("X-Logged-In", true, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
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
                }
            });

        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {
            authManager.Logout();

            Response.Cookies.Append("X-Access-Token", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(-60) });
            //Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(-60) });

            return Ok();
        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseModel request)
        {
            var authResponse = await authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            //Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }

        [HttpPost("googleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] OAuthLoginModel model)
        {
            var authResponse = await authManager.OAuthLogin(model);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            //Response.Cookies.Append("X-Logged-In", true, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

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
    }
}
