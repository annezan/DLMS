using DLMS_MODELS.Bases;
using DLMS_MODELS.ReadObjectCommandeDomain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadObjectCommandeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadObjectCommandeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Exécute la lecture d'objets pour une commande spécifique
        /// </summary>
        /// <param name="commandeId">ID de la commande à traiter</param>
        /// <returns>Résultat de l'exécution de la commande</returns>
        [HttpGet("execute/{commandeId}")]
        public async Task<ActionResult<ResponseBase<string>>> GetReadObjectCommande(int commandeId)
        {
            var query = new GetReadObjectCommandeQuery(commandeId);
            
            var result = await _mediator.Send(query);
            
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
