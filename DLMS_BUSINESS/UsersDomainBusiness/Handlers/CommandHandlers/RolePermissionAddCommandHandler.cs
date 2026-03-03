using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    /// <summary>
    /// Handler pour ajouter une ou plusieurs permissions à un rôle
    /// </summary>
    public class RolePermissionAddCommandHandler : IRequestHandler<RolePermissionAddCommand, ResponseBase<string>>
    {
        private readonly IRolePermissionCommandRepository _rolePermissionCommandRepository;
        private readonly IRolePermissionQueryRepository _rolePermissionQueryRepository;
        private readonly IRolesQueryRepository _rolesQueryRepository;
        private readonly IPermissionsQueryRepository _permissionsQueryRepository;

        public RolePermissionAddCommandHandler(
            IRolePermissionCommandRepository rolePermissionCommandRepository,
            IRolePermissionQueryRepository rolePermissionQueryRepository,
            IRolesQueryRepository rolesQueryRepository,
            IPermissionsQueryRepository permissionsQueryRepository)
        {
            _rolePermissionCommandRepository = rolePermissionCommandRepository;
            _rolePermissionQueryRepository = rolePermissionQueryRepository;
            _rolesQueryRepository = rolesQueryRepository;
            _permissionsQueryRepository = permissionsQueryRepository;
        }

        public async Task<ResponseBase<string>> Handle(RolePermissionAddCommand request, CancellationToken cancellationToken)
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

            // Vérifier que toutes les permissions existent
            var permissions = await _permissionsQueryRepository.GetAllAsync(p => 
                request.PermissionIds.Contains(p.Id) && p.DeletedAt == null);
            
            if (permissions.Count != request.PermissionIds.Count)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Une ou plusieurs permissions spécifiées n'existent pas.";
                return responseBase;
            }

            try
            {
                int addedCount = 0;
                var newRolePermissions = new List<RolePermission>();

                foreach (var permissionId in request.PermissionIds)
                {
                    // Vérifier si l'association existe déjà
                    var exists = await _rolePermissionQueryRepository.ExistsAsync(request.RoleId, permissionId);
                    
                    if (!exists)
                    {
                        newRolePermissions.Add(new RolePermission
                        {
                            RoleId = request.RoleId,
                            PermissionId = permissionId,
                            CreatedAt = DateTime.Now
                        });
                        addedCount++;
                    }
                }

                if (newRolePermissions.Any())
                {
                    await _rolePermissionCommandRepository.AddRangeAsync(newRolePermissions);
                    responseBase.Data = $"{addedCount} permission(s) ajoutée(s) au rôle '{role.Libelle}' avec succès.";
                }
                else
                {
                    responseBase.Data = "Toutes les permissions sont déjà associées à ce rôle.";
                }

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Une erreur est survenue lors de l'ajout des permissions : " + ex.Message;
                return responseBase;
            }
        }
    }
}

