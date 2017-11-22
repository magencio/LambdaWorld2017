using FunctionalLib.Extensions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWAPISDK.Helpers
{
    static class HttpHelper
    {
        public static async Task<T> GetAsync<T>(string query)
        {
            using (HttpClient httpClient = new HttpClient())
            using (HttpResponseMessage response = await httpClient.GetAsync(query))
            {
                return await 
                    response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadAsStringAsync()
                    .DeserializeAsync<T>();
            }
        }

        public static T Get<T>(string query)
            => GetAsync<T>(query).GetAwaiter().GetResult();
    }
}
