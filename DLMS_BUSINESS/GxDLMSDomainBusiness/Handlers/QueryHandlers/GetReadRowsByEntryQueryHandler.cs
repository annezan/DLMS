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
    public class GetReadRowsByEntryQueryHandler : IRequestHandler<GetReadRowsByEntryQuery, ResponseBase<ReadRowsByEntryResponse>>
    {
        private readonly IReadRowsByEntryQueryRepository _ReadRowsByEntryQueryRepository;

        public GetReadRowsByEntryQueryHandler(IReadRowsByEntryQueryRepository ReadRowsByEntryQueryRepository)
        {
            _ReadRowsByEntryQueryRepository = ReadRowsByEntryQueryRepository;
        }

        public async Task<ResponseBase<ReadRowsByEntryResponse>> Handle(GetReadRowsByEntryQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<ReadRowsByEntryResponse> responseBase = new ResponseBase<ReadRowsByEntryResponse>();
            try
            {
                var Read =  _ReadRowsByEntryQueryRepository.GetReadRowsByEntry(request.port, request.serialport, request.AddressIp, request.ClientAddress, request.SerialNumber, request.interfaceType, request.Objects);
                if (Read == null || Read == "Lecture impossible")
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Lecture impossible";
                    return responseBase;
                }
                responseBase.Data = ReadRowsByEntryMapper.Mapper.Map<ReadRowsByEntryResponse>(Read);
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
