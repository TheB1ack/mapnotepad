using Newtonsoft.Json;
using System;
using System.Diagnostics;
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

        #region -- IRestService implementation --

        public async Task<T> GetAsync<T>(string url)
        {
            T model;

            try
            {
                var json = await _httpClient.GetStringAsync(url);
                model = JsonConvert.DeserializeObject<T>(json);

            }
            catch(Exception e)
            {
                model = default;
                Debug.WriteLine(e.Message);
            }

            return model;
        }

        #endregion

    }
}
