using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MapNotepad.Services.REST
{
    public class RestService : IRestService
    {
        private readonly HttpClient _httpClient;
        public RestService()
        {
            _httpClient = new HttpClient();
        }

        #region -- IterfaceName implementation --

        public async Task<T> GetAsync<T>(string url)
        {
            var json = await _httpClient.GetStringAsync(url);
            var model = JsonConvert.DeserializeObject<T>(json);

            return model;
        }

        #endregion

    }
}
