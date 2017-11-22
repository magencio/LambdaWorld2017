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
    class Program
    {        
        static void Main(string[] args)
        {
            try
            {
                var swapi = new SWAPIClient();

                // Get specific entities
                swapi.GetPerson(1).Show();
                swapi.GetPlanet(1).Show();
                swapi.GetFilm(1).Show();
                swapi.GetSpecies(5).Show();
                swapi.GetVehicle(4).Show();
                swapi.GetStarship(15).Show();

                // Find entities
                swapi.GetPeople(query: "sky").Show();
                swapi.GetPlanets(query: "tat").Show();
                swapi.GetFilms(query: "hope").Show();
                swapi.GetSpecies(query: "wook").Show();
                swapi.GetVehicles(query: "speeder").Show();
                swapi.GetStarships(query: "mi").Show();

                // Find all people in a specific movie
                var film = swapi.GetFilms(query: "a new hope").First();
                swapi
                    .GetPeople(int.MaxValue)
                    .Where(person => person.Films.Contains(film.Url))
                    .Show();

                // Parse the url of an API call
                var (api, id) = "https://swapi.co/api/species/5/".GetApiAndId();
                Console.WriteLine($"{api}/{id}");
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
