using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.TypecommandeDomain.Queries;
using DLMS_MODELS.TypecommandeDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypecommandeController : ApiController
    {
        /// <summary>
        /// Récupère la liste des types de commandes
        /// </summary>
        [RequirePermission("VIEW_TYPECOMMANDE")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<TypecommandeResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetTypecommandeQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        // TODO: Implémenter les autres méthodes (add, edit, delete, getById) si nécessaire
    }
}
