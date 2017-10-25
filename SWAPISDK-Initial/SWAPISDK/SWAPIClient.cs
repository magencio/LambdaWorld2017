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

        // TODO:
        // - Interpolated strings
        // - Expression-bodied member
        // - Use HttpHelper.Get
        private async Task<T> GetSingleDataAsync<T>(string api, int id)
        {
            string url = string.Format("{0}/{1}/{2}", baseUri, api, id);
            return await HttpHelper.GetAsync<T>(url);
        }

        private T GetSingleData<T>(string api, int id)
        {
            return GetSingleDataAsync<T>(api, id).GetAwaiter().GetResult();
        }

        // TODO:
        // - LINQ (Language integrated query)
        // - Use HttpHelper.Get
        // - Extension method TakeUntilIncluding
        // - Expression-bodied member
        private async Task<IEnumerable<T>> GetPagedDataAsync<T>(string api, int maxPages = 1, string query = null)
        {
            List<T> results = new List<T>();

            if (maxPages >= 1)
            {
				PagedResults<T> pagedResults;
				int page = 1;
				do
				{
                    string url = string.Format("{0}/{1}/?page={2}&search={3}", baseUri, api, page, query);
					pagedResults = await HttpHelper.GetAsync<PagedResults<T>>(url);
					results.AddRange(pagedResults.Results);
					page++;
				}
				while (pagedResults.Count > results.Count && page < maxPages);
			}
            
            return results;
        }
        
        private IEnumerable<T> GetPagedData<T>(string api, int maxPages = 1, string query = null)
        {
            return GetPagedDataAsync<T>(api, maxPages, query).GetAwaiter().GetResult();
        }

        // TODO:
        // - Expression-bodied members
        public Person GetPerson(int id)
        {
            return GetSingleData<Person>("people", id);
        }

        public IEnumerable<Person> GetPeople(int maxPages = 1, string query = null)
        {
            return GetPagedData<Person>("people", maxPages, query);
        }

        public Planet GetPlanet(int id)
        {
            return GetSingleData<Planet>("planets", id);
        }

        public IEnumerable<Planet> GetPlanets(int maxPages = 1, string query = null)
        {
            return GetPagedData<Planet>("planets", maxPages, query);
        }

        public Film GetFilm(int id)
        {
            return GetSingleData<Film>("films", id);
        }

        public IEnumerable<Film> GetFilms(int maxPages = 1, string query = null)
        {
            return GetPagedData<Film>("films", maxPages, query);
        }

        public Species GetSpecies(int id)
        {
            return GetSingleData<Species>("species", id);
        }

        public IEnumerable<Species> GetSpecies(int maxPages = 1, string query = null)
        {
            return GetPagedData<Species>("species", maxPages, query);
        }

        public Vehicle GetVehicle(int id)
        {
            return GetSingleData<Vehicle>("vehicles", id);
        }

        public IEnumerable<Vehicle> GetVehicles(int maxPages = 1, string query = null)
        {
            return GetPagedData<Vehicle>("vehicles", maxPages, query);
        }

        public Starship GetStarship(int id)
        {
            return GetSingleData<Starship>("starships", id);
        }

        public IEnumerable<Starship> GetStarships(int maxPages = 1, string query = null)
        {
            return GetPagedData<Starship>("starships", maxPages, query);
        }
    }
}
