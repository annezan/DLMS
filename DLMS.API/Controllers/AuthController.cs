using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        [HttpPost("register")]
        [PermissionLabel("S'inscrire")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> Register([FromBody] UsersRegisterCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        [HttpPost("login")]
        [PermissionLabel("Se connecter")]
        public async Task<ActionResult<ResponseBase<TokenResponse>>> LoginAsync([FromBody] UsersLoginCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        [HttpPost("logout")]
        [Authorize]
        [PermissionLabel("Se deconnecter")]
        public async Task<ActionResult<ResponseBase<TokenResponse>>> LogoutAsync()
        {
            Guid UserId = GetUserIdFromToken();
            UsersLogoutCommand command = new UsersLogoutCommand();
            command.UserId = UserId;
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        [HttpPost("refreshtoken")]
        [Authorize]
        [PermissionLabel("Refraichire un token")]
        public async Task<ActionResult<ResponseBase<TokenResponse>>> RefreshToken([FromBody] UsersRefreshTokenCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        [HttpGet("getcurrentuser")]
        [Authorize]
        [PermissionLabel("Recupérer l'utilisateur connecté")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> GetCurrentUser()
        {
            Guid UserId = GetUserIdFromToken();
            var result = await Mediator.Send(new GetUsersByIdQuery(UserId));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        [HttpPost("change-password")]
        [PermissionLabel("Changer le mot de passe")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> ChangePassword([FromBody] UsersChangePasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        protected Guid GetUserIdFromToken()
        {
            Guid UserId = Guid.Empty;
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        string strUserId = identity.FindFirst(ClaimsKey.UtilisateurId).Value;
                        Guid.TryParse(strUserId, out UserId);
                    }
                }
                return UserId;
            }
            catch
            {
                return UserId;
            }
        }

        protected async Task DeleteUserConnectedAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HttpContext.Response.WriteAsync("Token révoqué");
                return;
            }
        }
    }

}
