using DLMS_MODELS.Bases;
using DLMS_MODELS.GxDLMSDomain.Queries;
using DLMS_MODELS.GxDLMSDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadRowsByEntryController : ApiController
    {


        [HttpGet("getReadRowsByEntry")]
        //[Authorize]
        public async Task<ActionResult<ResponseBase<ReadRowsByEntryResponse>>> GetReadRowsByEntry([FromQuery] GetReadRowsByEntryQuery query)
        {
            var result = await Mediator.Send(new GetReadRowsByEntryQuery(query.port, query.serialport, query.AddressIp, query.ClientAddress, query.SerialNumber, query.interfaceType, query.Objects));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }

}
