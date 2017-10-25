using FunctionalLib.Extensions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWAPISDK.Helpers
{
    static class HttpHelper
    {
        // TODO:
        // - Create sync version
        // - Expression-bodied member
        // - Compact using statements
        // - Extension method DeserializeAsync
        public static async Task<T> GetAsync<T>(string query)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(query))
                {
                    response.EnsureSuccessStatusCode();

                    HttpContent httpContent = response.Content;
                    string content = await httpContent.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
                    {
                        Error = (sender, errorArgs) => errorArgs.ErrorContext.Handled = true
                    });
                }
            }
        }
    }
}
