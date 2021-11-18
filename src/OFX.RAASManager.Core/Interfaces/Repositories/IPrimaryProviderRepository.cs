using System.Threading.Tasks;

namespace OFX.RAASManager.Core.Interfaces.Repositories
{
    public interface IPrimaryProviderRepository
    {
        Task SetPrimaryProvider(string primaryProvider);
        string GetPrimaryProvider();
    }
}
