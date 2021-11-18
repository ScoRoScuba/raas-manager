using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Interfaces.Services;

namespace OFX.RAASManager.Application.Services
{
    public class PrimaryRateProviderService : IPrimaryRateProviderService
    {
        private readonly IPrimaryProviderRepository _primaryProviderRepository;

        public PrimaryRateProviderService(IPrimaryProviderRepository primaryProviderRepository)
        {
            _primaryProviderRepository = primaryProviderRepository;
        }

        public void SetPrimaryRateProvider(string primaryProvider)
        {
            _primaryProviderRepository.SetPrimaryProvider(primaryProvider);
        }

        public string GetPrimaryRateProvider()
        {
            return _primaryProviderRepository.GetPrimaryProvider();
        }
    }
}
