using FunctionalLib.Extensions;
using SWAPISDK.Entities;
using SWAPISDK.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWAPISDK
{
    // Star Wars API: https://swapi.co/api/
    // Star Wars API docs: https://swapi.co/documentation
    // Other Star Wars API clients: https://github.com/olcay/SharpTrooper
    public class SWAPIClient
    {
        private const string baseUri = "https://swapi.co/api";

        private T GetSingleData<T>(string api, int id)
            => HttpHelper.Get<T>($"{baseUri}/{api}/{id}");

        private IEnumerable<T> GetPagedData<T>(string api, int maxPages = 1, string query = null)
            => Enumerable
                .Range(1, maxPages)
                .Select(page => HttpHelper.Get<PagedResults<T>>($"{baseUri}/{api}/?page={page}&search={query}"))
                .TakeUntilIncluding(pagedResults => pagedResults.Next == null)
                .SelectMany(pagedResults => pagedResults.Results)
                .ToList();
        
        public Person GetPerson(int id)
            => GetSingleData<Person>("people", id);

        public IEnumerable<Person> GetPeople(int maxPages = 1, string query = null)
            => GetPagedData<Person>("people", maxPages, query);

        public Planet GetPlanet(int id)
            => GetSingleData<Planet>("planets", id);

        public IEnumerable<Planet> GetPlanets(int maxPages = 1, string query = null)
            => GetPagedData<Planet>("planets", maxPages, query);

        public Film GetFilm(int id)
            => GetSingleData<Film>("films", id);

        public IEnumerable<Film> GetFilms(int maxPages = 1, string query = null)
            => GetPagedData<Film>("films", maxPages, query);

        public Species GetSpecies(int id)
            => GetSingleData<Species>("species", id);

        public IEnumerable<Species> GetSpecies(int maxPages = 1, string query = null)
            => GetPagedData<Species>("species", maxPages, query);

        public Vehicle GetVehicle(int id)
            => GetSingleData<Vehicle>("vehicles", id);

        public IEnumerable<Vehicle> GetVehicles(int maxPages = 1, string query = null)
            => GetPagedData<Vehicle>("vehicles", maxPages, query);

        public Starship GetStarship(int id)
            => GetSingleData<Starship>("starships", id);

        public IEnumerable<Starship> GetStarships(int maxPages = 1, string query = null)
            => GetPagedData<Starship>("starships", maxPages, query);
    }
}
