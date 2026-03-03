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
    public class GetReadObjectProfileQueryHandler : IRequestHandler<GetReadObjectProfileQuery, ResponseBase<ReadObjectProfileResponse>>
    {
        private readonly IReadObjectProfileQueryRepository _ReadObjectProfileQueryRepository;

        public GetReadObjectProfileQueryHandler(IReadObjectProfileQueryRepository ReadObjectProfileQueryRepository)
        {
            _ReadObjectProfileQueryRepository = ReadObjectProfileQueryRepository;
        }

        public async Task<ResponseBase<ReadObjectProfileResponse>> Handle(GetReadObjectProfileQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<ReadObjectProfileResponse> responseBase = new ResponseBase<ReadObjectProfileResponse>();
            try
            {
                var Read = await _ReadObjectProfileQueryRepository.GetReadObjectProfile(request.port, request.serialport, request.AddressIp, request.ClientAddress, request.SerialNumber, request.interfaceType, request.Objects);
                if (Read == null || Read == "Lecture impossible")
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Lecture impossible";
                    return responseBase;
                }
                responseBase.Data = ReadObjectProfileMapper.Mapper.Map<ReadObjectProfileResponse>(Read);
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
