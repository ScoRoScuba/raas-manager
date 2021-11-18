using System.Net.Http;
using System.Threading.Tasks;
using OFX.RAASManager.Core.Interfaces;

namespace OFX.RAASManager.Core
{
    public class RAASHttpClient : IHttpClient 
    {
        private readonly HttpClient _httpClient;

        public RAASHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetStringAsync(string requestUri)
        {
            return _httpClient.GetStringAsync(requestUri);
        }
    }
}
