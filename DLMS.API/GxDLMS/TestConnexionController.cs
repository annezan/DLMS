using DLMS_MODELS.Bases;
using DLMS_MODELS.GxDLMSDomain.Queries;
using DLMS_MODELS.GxDLMSDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestConnexionController : ApiController
    {


        [HttpGet("getTestConnexion")]
        //[Authorize]
        public async Task<ActionResult<ResponseBase<TestConnexionResponse>>> GetTestConnexion([FromQuery] GetTestConnexionQuery query)
        {
            var result = await Mediator.Send(new GetTestConnexionQuery(query.port, query.serialport, query.AddressIp, query.ClientAddress, query.SerialNumber, query.interfaceType, query.password, query.AuthenticationKey, query.UnicastKey, query.Objects));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }

}
