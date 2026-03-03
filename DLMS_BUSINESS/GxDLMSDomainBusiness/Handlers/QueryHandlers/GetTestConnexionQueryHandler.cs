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

namespace DLMS_BUSINESS.TestConnexionDomainBusiness.Handlers.QueryHandlers
{
    public class GetTestConnexionQueryHandler : IRequestHandler<GetTestConnexionQuery, ResponseBase<TestConnexionResponse>>
    {
        private readonly ITestConnexionQueryRepository _TestConnexionQueryRepository;

        public GetTestConnexionQueryHandler(ITestConnexionQueryRepository TestConnexionQueryRepository)
        {
            _TestConnexionQueryRepository = TestConnexionQueryRepository;
        }

        public async Task<ResponseBase<TestConnexionResponse>> Handle(GetTestConnexionQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<TestConnexionResponse> responseBase = new ResponseBase<TestConnexionResponse>();
            try
            {
                var TestConnexion =  _TestConnexionQueryRepository.GetTestConnexion(request.port, request.serialport, request.AddressIp, request.ClientAddress, request.SerialNumber, request.interfaceType, request.password, request.AuthenticationKey, request.UnicastKey, request.Objects);
                if (TestConnexion == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Connexion impossible";
                    return responseBase;
                }
                responseBase.Data = TestConnexionMapper.Mapper.Map<TestConnexionResponse>(TestConnexion);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Connexion impossible" + ex.Message;
                return responseBase;
            }
        }
    }
}
