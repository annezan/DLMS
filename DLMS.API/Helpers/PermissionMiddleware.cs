using System.Security.Claims;
using DLMS_DAL.Datas;
using DLMS_MODELS.Bases;
using Microsoft.EntityFrameworkCore;

namespace DLMS.API.Helpers
{
    public class PermissionMiddleware
    {

        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, DLMSDBContext dbContext)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();
            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var path = context.Request.Path.ToString().TrimStart('/');
                var method = context.Request.Method;
                var action = $"{method}:{path}"; // Exemple: "GET:/api/users"

                var roleId = await dbContext.Roles
                    .Where(r => r.Code == role)
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                if (roleId == Guid.Empty)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Accès refusé : Rôle inconnu.";
                    await context.Response.WriteAsJsonAsync<ResponseBase<string>>(responseBase);
                    return;
                }

                var hasPermission = await dbContext.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.Permissions.Action.ToLower() == action.ToLower());

                if (!hasPermission)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Accès refusé : Vous n'avez pas les permissions pour cette action.";
                    await context.Response.WriteAsJsonAsync<ResponseBase<string>>(responseBase);
                    return;
                }
            }

            await _next(context);
        }
    }

}

