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
    }
}