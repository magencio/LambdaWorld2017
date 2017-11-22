using System;

namespace SWAPISDK.Extensions
{
    public static class StringExtensions
    {
        // Valid url: https://swapi.co/api/{api}/{id}/
        // TODO: 
        // - Refactor
        // - Conditional operator ?:
        // - Pattern matching (is-expressions with patterns)
        // - Type inference
        // - Throw expressions
        // - nameof expressions
        // - Out variables
        // - Tuples 
        // - Extension method
        // - Expression-bodied member
        public static Tuple<string, int> GetApiAndId(string entityUrl)
        {
            if (entityUrl != null)
            {
                Uri uri = new Uri(entityUrl);
                if (uri.Authority.Equals("swapi.co"))
                {
                    string[] pathFragments = uri.AbsolutePath.Split(new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries);

                    if (pathFragments.Length == 3 && pathFragments[0].Equals("api"))
                    {
                        int id;
                        if (int.TryParse(pathFragments[2], out id))
                        {
                            return Tuple.Create(pathFragments[1], id);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid entity Id");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid API url");
                    }
                }
                else 
                {
                    throw new ArgumentException("Unknown web service");
                }
            }
            else
            {
                throw new ArgumentNullException("entityUrl");
            }
        }

        // // Valid url: https://swapi.co/api/{api}/{id}/
        // public static (string api, int id) GetApiAndId(this string entityUrl)
        //     => entityUrl.ToUri().GetPathFragments().GetApiAndId();

        // private static Uri ToUri(this string entityUrl)
        //     => (entityUrl != null)
        //         ? (new Uri(entityUrl) is var uri && uri.Authority.Equals("swapi.co"))
        //             ? uri 
        //             : throw new ArgumentException("Unknown web service") 
        //         : throw new ArgumentNullException(nameof(entityUrl));

        // private static string[] GetPathFragments(this Uri uri)
        //     => (uri.AbsolutePath.Split(new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries) is var pathFragments
        //         && pathFragments.Length == 3 
        //         && pathFragments[0].Equals("api")) 
        //         ? pathFragments 
        //         : throw new ArgumentException("Invalid API url");

        // private static int ToEntityId(this string fragment)
        //     =>  int.TryParse(fragment, out var id) 
        //         ? id 
        //         : throw new ArgumentException("Invalid entity Id");
                
        // private static (string api, int id) GetApiAndId(this string[] pathFragments)
        //     => (api: pathFragments[1], id: pathFragments[2].ToEntityId());
    }
}