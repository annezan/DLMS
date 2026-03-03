using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    /// <summary>
    /// Handler pour retirer une ou plusieurs permissions d'un rôle
    /// </summary>
    public class RolePermissionDeleteCommandHandler : IRequestHandler<RolePermissionDeleteCommand, ResponseBase<string>>
    {
        private readonly IRolePermissionCommandRepository _rolePermissionCommandRepository;
        private readonly IRolePermissionQueryRepository _rolePermissionQueryRepository;
        private readonly IRolesQueryRepository _rolesQueryRepository;

        public RolePermissionDeleteCommandHandler(
            IRolePermissionCommandRepository rolePermissionCommandRepository,
            IRolePermissionQueryRepository rolePermissionQueryRepository,
            IRolesQueryRepository rolesQueryRepository)
        {
            _rolePermissionCommandRepository = rolePermissionCommandRepository;
            _rolePermissionQueryRepository = rolePermissionQueryRepository;
            _rolesQueryRepository = rolesQueryRepository;
        }

        public async Task<ResponseBase<string>> Handle(RolePermissionDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();

            // Vérifier que le rôle existe
            var role = await _rolesQueryRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Le rôle spécifié n'existe pas.";
                return responseBase;
            }

            // Vérifier que des permissions ont été fournies
            if (request.PermissionIds == null || !request.PermissionIds.Any())
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Aucune permission spécifiée.";
                return responseBase;
            }

            try
            {
                // Supprimer les associations rôle-permission
                await _rolePermissionCommandRepository.DeleteRangeByRoleAndPermissionsAsync(
                    request.RoleId, 
                    request.PermissionIds);

                responseBase.Data = $"Permission(s) retirée(s) du rôle '{role.Libelle}' avec succès.";
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Une erreur est survenue lors de la suppression des permissions : " + ex.Message;
                return responseBase;
            }
        }
    }
}

