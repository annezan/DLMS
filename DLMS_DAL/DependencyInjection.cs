using DLMS_DAL.AlarmsDomainDal.Repositories.Queries;
using DLMS_DAL.AssociationKeyDomainDal.Repositories;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Commands;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using DLMS_DAL.Bases;
using DLMS_DAL.CelluleDomainDal.Repositories;
using DLMS_DAL.CelluleDomainDal.Repositories.Commands;
using DLMS_DAL.CelluleDomainDal.Repositories.Queries;
using DLMS_DAL.CodeObisDomainDal.Repositories.Queries;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_DAL.CommandeDomainDal.Repositories;
using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.CompteurDomainDal.Repositories;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries;
using DLMS_DAL.CompteurCelluleDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurCelluleDomainDal.Repositories.Queries;
using DLMS_DAL.EquipementDomainDal.Repositories;
using DLMS_DAL.EquipementDomainDal.Repositories.Commands;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_DAL.ErrorDomainDal.Repositories.Queries;
using DLMS_DAL.EventsDomainDal.Repositories.Queries;
using DLMS_DAL.FabricantDomainDal.Repositories;
using DLMS_DAL.FabricantDomainDal.Repositories.Commands;
using DLMS_DAL.FabricantDomainDal.Repositories.Queries;
using DLMS_DAL.FabricantsDomainDal.Repositories.Queries;
using DLMS_DAL.GxdlmsprofilgenericdetailDomainDal.Repositories;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_DAL.PosteDomainDal.Repositories.Commands;
using DLMS_DAL.PosteDomainDal.Repositories;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_DAL.TypecommandeDomainDal.Repositories.Queries;
using DLMS_DAL.UsersDomainDal.Repositories;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using Microsoft.Extensions.DependencyInjection;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.ReadingDomainDal.Repositories.Commands;
using DLMS_DAL.ReadingDomainDal.Repositories.Queries;
using DLMS_DAL.Services;

namespace DLMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IQueryBaseRepository<>), typeof(QueryBaseRepository<>));
            services.AddTransient<IUsersQueryRepository, UsersQueryRepository>();
            services.AddTransient<IUsersTokenCommandRepository, UsersTokenCommandRepository>();
            services.AddTransient<IRolesCommandRepository, RolesCommandRepository>();
            services.AddTransient<IRolePermissionCommandRepository, RolePermissionCommandRepository>();

            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
            services.AddTransient<IUsersCommandRepository, UsersCommandRepository>();
            services.AddTransient<IUsersTokenQueryRepository, UsersTokenQueryRepository>();
            services.AddTransient<IRolesQueryRepository, RolesQueryRepository>();
            services.AddTransient<IPermissionsQueryRepository, PermissionsQueryRepository>();
            services.AddTransient<IRolePermissionQueryRepository, RolePermissionQueryRepository>();

            services.AddTransient<IAlarmsQueryRepository, AlarmsQueryRepository>();
            services.AddTransient<IAssociationKeyQueryRepository, AssociationKeyQueryRepository>();
            services.AddTransient<IAssociationKeyCommandRepository, AssociationKeyCommandRepository>();
            services.AddTransient<ICodeObisQueryRepository, CodeObisQueryRepository>();
            services.AddTransient<ICommandeCompteurCommandRepository, CommandeCompteurCommandRepository>();
            services.AddTransient<ICommandeCompteurQueryRepository, CommandeCompteurQueryRepository>();
            services.AddTransient<IResultatCommandeCompteurCommandRepository, ResultatCommandeCompteurCommandRepository>();
            services.AddTransient<IResultatCommandeCompteurQueryRepository, ResultatCommandeCompteurQueryRepository>();
            services.AddTransient<ICommandeCommandRepository, CommandeCommandRepository>();
            services.AddTransient<ICommandeOrchestrationService, CommandeOrchestrationService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICommandeQueryRepository, CommandeQueryRepository>();
            services.AddTransient<ICompteurCommandRepository, CompteurCommandRepository>();
            services.AddTransient<ICompteurQueryRepository, CompteurQueryRepository>();

            services.AddTransient<ICompteurEquipementCommandRepository, CompteurEquipementCommandRepository>();
            services.AddTransient<IEquipementCommandRepository, EquipementCommandRepository>();
            services.AddTransient<IEquipementQueryRepository, EquipementQueryRepository>();
            services.AddTransient<IErrorQueryRepository, ErrorQueryRepository>();
            services.AddTransient<IEventsQueryRepository, EventsQueryRepository>();
            services.AddTransient<IFabricantCommandRepository, FabricantCommandRepository>();
            services.AddTransient<IFabricantQueryRepository, FabricantQueryRepository>();
            services.AddTransient<IGxdlmsprofilgenericdetailCommandRepository, GxdlmsprofilgenericdetailCommandRepository>();
            services.AddTransient<IGxdlmsprofilgenericdetailQueryRepository, GxdlmsprofilgenericdetailQueryRepository>();
            services.AddTransient<IGxdlmsprofilgenericdetailseventCommandRepository, GxdlmsprofilgenericdetailseventCommandRepository>();
            services.AddTransient<IGxdlmsprofilgenericdetailseventQueryRepository, GxdlmsprofilgenericdetailseventQueryRepository>();
            services.AddTransient<IGxdlmsprofilgenericQueryRepository, GxdlmsprofilgenericQueryRepository>();
            services.AddTransient<IPosteQueryRepository, PosteQueryRepository>();
            services.AddTransient<IPosteCommandRepository, PosteCommandRepository>();
            services.AddTransient<ICelluleQueryRepository, CelluleQueryRepository>();
            services.AddTransient<ICelluleCommandRepository, CelluleCommandRepository>();

            services.AddTransient<ITypecommandeQueryRepository, TypecommandeQueryRepository>();
            services.AddTransient<ICompteurEquipementCommandRepository, CompteurEquipementCommandRepository>();
            services.AddTransient<ICompteurEquipementQueryRepository, CompteurEquipementQueryRepository>();
            services.AddTransient<ICompteurCelluleCommandRepository, CompteurCelluleCommandRepository>();
            services.AddTransient<ICompteurCelluleQueryRepository, CompteurCelluleQueryRepository>();

            // Reading Domain (Multi-Pass)
            services.AddTransient<IReadingCycleQueryRepository, ReadingCycleQueryRepository>();
            services.AddTransient<IMeterReadingStatusQueryRepository, MeterReadingStatusQueryRepository>();
            services.AddTransient<IReadingSessionCommandRepository, ReadingSessionCommandRepository>();

            services.AddTransient<IReadQueryRepository, ReadQueryRepository>();
            services.AddTransient<IReadObjectProfileQueryRepository, ReadObjectProfileQueryRepository>();
            services.AddTransient<IReadObjectCommandeQueryRepository, ReadObjectCommandeQueryRepository>();
            services.AddTransient<ITestConnexionQueryRepository, TestConnexionQueryRepository>();
            services.AddTransient<IReadRowsByRangeQueryRepository, ReadRowsByRangeQueryRepository>();
            services.AddTransient<IReadRowsByEntryQueryRepository, ReadRowsByEntryQueryRepository>();

            return services;
        }
    }
}
