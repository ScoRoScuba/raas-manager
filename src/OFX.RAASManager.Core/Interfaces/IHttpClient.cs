using System.Threading.Tasks;

namespace OFX.RAASManager.Core.Interfaces
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string requestUri);
    }
}
