using System;

namespace SWAPISDK.Entities
{
    // https://swapi.co/api/films/schema
    public class Film : Entity
    {
        public string Title { get; set; }
        public int Episode_id { get; set; }
        public string Opening_crawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string Release_date { get; set; }
        public string[] Characters { get; set; }
        public string[] Planets { get; set; }
        public string[] Starships { get; set; }
        public string[] Vehicles { get; set; }
        public string[] Species { get; set; }
    }
}
