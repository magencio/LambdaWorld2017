using FunctionalLib.Extensions;
using SWAPISDK;
using SWAPISDK.Entities;
using SWAPISDK.Extensions;
using SWAPISDKTest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWAPISDKTest
{
    // TODO:
    // - Latest C# support in all projects
    class Program
    {
        // TODO:
        // - Extension methods Show and Show(IEnumerable)
        public static void Show(Entity entity)
        {
            if (entity is Person)
            {
                Person person = entity as Person;
                Console.WriteLine(string.Format("[Person] {0}", person.Name));
            }
            else if (entity is Planet)
            {
                Planet planet = entity as Planet;
                Console.WriteLine(string.Format("[Planet] {0}", planet.Name));
            }
            else if (entity is Film)
            {
                Film film = entity as Film;
                Console.WriteLine(string.Format("[Film] {0}", film.Title));
            }
            else if (entity is Species)
            {
                Species species = entity as Species;
                Console.WriteLine(string.Format("[Species] {0}", species.Name));
            }
            else if (entity is Vehicle)
            {
                Vehicle vehicle = entity as Vehicle;
                Console.WriteLine(string.Format("[Vehicle] {0}", vehicle.Name));
            }
            else if (entity is Starship)
            {
                Starship starship = entity as Starship;
                if (starship.Name.Equals("Millennium Falcon"))
                {
                    Console.WriteLine(string.Format("[Starship] {0} --> my favourite starship ever!!!", starship.Name));                    
                }
                else
                {
                    Console.WriteLine(string.Format("[Starship] {0}", starship.Name));
                }
            }
        }

        public static void Show(IEnumerable<Entity> entities)
        {
            foreach(Entity entity in entities)
            {
                Show(entity);
            }
        }
        
        static void Main(string[] args)
        {
            try
            {
                // TODO:
                // - Type inference
                SWAPIClient swapi = new SWAPIClient();

                // var swapi = new SWAPIClient();

                // Get specific entities
                // TODO:
                // - Extension method Show
                Show(swapi.GetPerson(1));
                Show(swapi.GetPlanet(1));
                Show(swapi.GetFilm(1));
                Show(swapi.GetSpecies(5));
                Show(swapi.GetVehicle(4));
                Show(swapi.GetStarship(15));

                // swapi.GetPerson(1).Show();
                // swapi.GetPlanet(1).Show();
                // swapi.GetFilm(1).Show();
                // swapi.GetSpecies(5).Show();
                // swapi.GetVehicle(4).Show();
                // swapi.GetStarship(15).Show();

                // Find entities
                // TODO:
                // - Extension method Show
                Show(swapi.GetPeople(query: "sky"));
                Show(swapi.GetPlanets(query: "tat"));
                Show(swapi.GetFilms(query: "hope"));
                Show(swapi.GetSpecies(query: "wook"));
                Show(swapi.GetVehicles(query: "speeder"));
                Show(swapi.GetStarships(query: "mi"));

                // swapi.GetPeople(query: "sky").Show();
                // swapi.GetPlanets(query: "tat").Show();
                // swapi.GetFilms(query: "hope").Show();
                // swapi.GetSpecies(query: "wook").Show();
                // swapi.GetVehicles(query: "speeder").Show();
                // swapi.GetStarships(query: "mi").Show();

                // Find all people in a specific movie
                // TODO: 
                // - LINQ (Language-Integrated Query)
                // - Type inference
                // - Extension method Show
                Film film = swapi.GetFilms(query: "a new hope").First();
                IEnumerable<Person> people = swapi.GetPeople(int.MaxValue);
                foreach (Person person in people)
                {
                    foreach (string url in person.Films)
                    {
                        if (url.Equals(film.Url))
                        {
                            Show(person);
                        }
                    }
                }

                // var film = swapi.GetFilms(query: "a new hope").First();
                // swapi
                //     .GetPeople(int.MaxValue)
                //     .Where(person => person.Films.Contains(film.Url))
                //     .Show();

                // Parse the url of an API call
                // TODO: 
                // - Extension method GetApiAndId
                // - Tuples
                // - Interpolated strings                
                Tuple<string, int> apiAndId = SWAPISDK.Extensions.StringExtensions.GetApiAndId("https://swapi.co/api/species/5/");
                string api = apiAndId.Item1;
                int id = apiAndId.Item2;
                Console.WriteLine(string.Format("{0}/{1}", api, id));

                // var (api, id) = "https://swapi.co/api/species/5/".GetApiAndId();
                // Console.WriteLine($"{api}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("\n<< Press Enter to exit >>");
                Console.Read();
            }
        }
    }
}
