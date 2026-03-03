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
    public class GetReadQueryHandler : IRequestHandler<GetReadQuery, ResponseBase<ReadResponse>>
    {
        private readonly IReadQueryRepository _ReadQueryRepository;

        public GetReadQueryHandler(IReadQueryRepository ReadQueryRepository)
        {
            _ReadQueryRepository = ReadQueryRepository;
        }

        public async Task<ResponseBase<ReadResponse>> Handle(GetReadQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<ReadResponse> responseBase = new ResponseBase<ReadResponse>();
            try
            {
                var Read = await _ReadQueryRepository.GetRead(request.port, request.serialport, request.AddressIp, request.ClientAddress, request.SerialNumber, request.interfaceType, request.Objects);
                if (Read == null || Read == "Lecture impossible")
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Lecture impossible";
                    return responseBase;
                }
                responseBase.Data = ReadMapper.Mapper.Map<ReadResponse>(Read);
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
