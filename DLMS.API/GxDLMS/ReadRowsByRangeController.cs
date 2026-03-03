using DLMS_MODELS.Bases;
using DLMS_MODELS.GxDLMSDomain.Queries;
using DLMS_MODELS.GxDLMSDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadRowsByRangeController : ApiController
    {


        [HttpGet("getReadRowsByRange")]
        //[Authorize]
        public async Task<ActionResult<ResponseBase<ReadRowsByRangeResponse>>> GetReadRowsByRange([FromQuery] GetReadRowsByRangeQuery query)
        {
            var result = await Mediator.Send(new GetReadRowsByRangeQuery(query.datestart, query.dateend, query.port, query.serialport, query.AddressIp, query.ClientAddress, query.SerialNumber, query.interfaceType, query.Objects));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }

}
