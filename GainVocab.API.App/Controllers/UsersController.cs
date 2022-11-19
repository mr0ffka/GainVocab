using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Users;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GainVocab.API.App.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthManager AuthManager;
        private readonly UserManager<APIUser> UserManager;
        private readonly ILogger<UsersController> Logger;

        public UsersController(IAuthManager authManager, UserManager<APIUser> userManager, ILogger<UsersController> logger)
        {
            this.AuthManager = authManager;
            this.Logger = logger;
            this.UserManager = userManager;
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
                Logger.LogInformation($"[api/users/me] {User.Identity.Name}");

                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var roleClaims = User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                var uidClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
                var roles = new List<string>();
                roleClaims.ForEach(r => roles.Add(r.Value));

                var frontUser = new FrontUserModel
                {
                    IsAuthenticated = User.Identity.IsAuthenticated,
                    Email = emailClaim != null ? emailClaim.Value : null,
                    Roles = roles,
                    Id = uidClaim != null ? uidClaim.Value : null,
                    IsAdmin = roles.Any(r => r == Enum.GetName(typeof(UserRoles), UserRoles.Administrator)),
                };

                return Ok(frontUser);
            }

            return Unauthorized();
        }
    }
}
