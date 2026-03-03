using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;

namespace DLMS_SERVICE.Services
{
    public interface IDataProcessingServiceFactory
    {
        IDataProcessingService Create();
    }

    public class DataProcessingServiceFactory : IDataProcessingServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataProcessingServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IDataProcessingService Create()
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataProcessingService>>();
            var compteurCommandRepo = scope.ServiceProvider.GetRequiredService<ICompteurCommandRepository>();
            var compteurQueryRepo = scope.ServiceProvider.GetRequiredService<ICompteurQueryRepository>();
            
            return new DataProcessingService(logger, compteurCommandRepo, compteurQueryRepo);
        }
    }
}
