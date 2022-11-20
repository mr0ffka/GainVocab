using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace GainVocab.API.App.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager AuthManager;
        private readonly ILogger<AuthController> Logger;

        public AuthController(IAuthManager authManager, ILogger<AuthController> logger)
        {
            AuthManager = authManager;
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

            if (authResponse == null)
            {
                return Unauthorized();
            }

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
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseModel request)
        {
            var authResponse = await AuthManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }

        [HttpPost("googlelogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] OAuthLoginModel model)
        {
            var authResponse = await AuthManager.OAuthLogin(model);

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

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

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
    }
}
