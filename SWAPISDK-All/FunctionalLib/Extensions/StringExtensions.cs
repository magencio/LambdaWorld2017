using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FunctionalLib.Extensions
{
    public static class StringExtensions
    {
        // TODO:
        // - Extension method DeserializeAsync
        // - Expression-bodied member

        // public static async Task<T> DeserializeAsync<T>(this Task<string> content)
        //     => JsonConvert.DeserializeObject<T>(await content, new JsonSerializerSettings
        //         {
        //             Error = (sender, errorArgs) => errorArgs.ErrorContext.Handled = true
        //         });
    }
}
