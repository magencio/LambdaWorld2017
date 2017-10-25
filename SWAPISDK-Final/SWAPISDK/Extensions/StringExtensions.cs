using System;

namespace SWAPISDK.Extensions
{
    public static class StringExtensions
    {
        private static Uri ToUri(this string entityUrl)
            => (entityUrl != null)
                ? (new Uri(entityUrl) is var uri && uri.Authority.Equals("swapi.co"))
                    ? uri 
                    : throw new ArgumentException("Unknown web service") 
                : throw new ArgumentNullException(nameof(entityUrl));

        private static string[] GetPathFragments(this Uri uri)
            => (uri.AbsolutePath.Split(new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries) is var pathFragments
                && pathFragments.Length == 3 
                && pathFragments[0].Equals("api")) 
                ? pathFragments 
                : throw new ArgumentException("Invalid API url");

        private static int ToEntityId(this string fragment)
            =>  int.TryParse(fragment, out var id) 
                ? id 
                : throw new ArgumentException("Invalid entity Id");
                
        private static (string api, int id) GetApiAndId(this string[] pathFragments)
            => (api: pathFragments[1], id: pathFragments[2].ToEntityId());

        // Valid url: https://swapi.co/api/{api}/{id}/
        public static (string api, int id) GetApiAndId(this string entityUrl)
            => entityUrl.ToUri().GetPathFragments().GetApiAndId();
    }
}