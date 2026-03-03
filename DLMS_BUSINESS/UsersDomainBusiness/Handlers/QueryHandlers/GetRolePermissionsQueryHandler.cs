using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    /// <summary>
    /// Handler pour récupérer toutes les permissions d'un rôle
    /// </summary>
    public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, ResponseBase<List<PermissionResponse>>>
    {
        private readonly IRolePermissionQueryRepository _rolePermissionQueryRepository;
        private readonly IRolesQueryRepository _rolesQueryRepository;

        public GetRolePermissionsQueryHandler(
            IRolePermissionQueryRepository rolePermissionQueryRepository,
            IRolesQueryRepository rolesQueryRepository)
        {
            _rolePermissionQueryRepository = rolePermissionQueryRepository;
            _rolesQueryRepository = rolesQueryRepository;
        }

        public async Task<ResponseBase<List<PermissionResponse>>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<PermissionResponse>> responseBase = new ResponseBase<List<PermissionResponse>>();

            try
            {
                // Vérifier que le rôle existe
                var role = await _rolesQueryRepository.GetByIdAsync(request.RoleId);
                if (role == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Le rôle spécifié n'existe pas.";
                    return responseBase;
                }

                // Récupérer les permissions du rôle
                var permissions = await _rolePermissionQueryRepository.GetPermissionsByRoleIdAsync(request.RoleId);

                // Mapper les permissions vers PermissionResponse
                responseBase.Data = PermissionMapper.Mapper.Map<List<PermissionResponse>>(permissions);
                responseBase.Message = $"Permissions du rôle '{role.Libelle}' récupérées avec succès.";

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Une erreur est survenue lors de la récupération des permissions : " + ex.Message;
                return responseBase;
            }
        }
    }
}

