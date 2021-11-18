namespace OFX.RAASManager.Core.Interfaces.Services
{
    public interface IPrimaryRateProviderService
    {
        void SetPrimaryRateProvider(string provider);
        string GetPrimaryRateProvider();
    }
}