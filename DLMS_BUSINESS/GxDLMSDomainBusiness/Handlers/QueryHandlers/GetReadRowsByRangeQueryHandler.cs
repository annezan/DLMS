using DLMS_BUSINESS.GxDLMSDomainBusiness.Mappers;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_MODELS.GxDLMSDomain.Queries;
using DLMS_MODELS.GxDLMSDomain.Responses;
using Gurux.DLMS.Enums;
using MediatR;
using System.IO.Ports;

namespace DLMS_BUSINESS.ReadDomainBusiness.Handlers.QueryHandlers
{
    public class GetReadRowsByRangeQueryHandler : IRequestHandler<GetReadRowsByRangeQuery, ResponseBase<ReadRowsByRangeResponse>>
    {
        private readonly IReadRowsByRangeQueryRepository _ReadRowsByRangeQueryRepository;

        public GetReadRowsByRangeQueryHandler(IReadRowsByRangeQueryRepository ReadRowsByRangeQueryRepository)
        {
            _ReadRowsByRangeQueryRepository = ReadRowsByRangeQueryRepository;
        }

        public async Task<ResponseBase<ReadRowsByRangeResponse>> Handle(GetReadRowsByRangeQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<ReadRowsByRangeResponse> responseBase = new ResponseBase<ReadRowsByRangeResponse>();
            try
            {
                var Read =  _ReadRowsByRangeQueryRepository.GetReadRowsByRange(request.datestart,request.dateend, request.port, request.serialport, request.AddressIp, request.ClientAddress, request.SerialNumber, request.interfaceType, request.Objects);
                if (Read == null || Read == "Lecture impossible")
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Lecture impossible";
                    return responseBase;
                }
                responseBase.Data = ReadRowsByRangeMapper.Mapper.Map<ReadRowsByRangeResponse>(Read);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Lecture impossible" + ex.Message;
                return responseBase;
            }
        }
    }
}
