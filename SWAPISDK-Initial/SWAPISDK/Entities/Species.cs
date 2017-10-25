using System;

namespace SWAPISDK.Entities
{
    // https://swapi.co/api/species/schema
    public class Species : Entity
    {
        public string Name { get; set; }
        public string Classification { get; set; }
        public string Designation { get; set; }
        public string Average_height { get; set; }
        public string Skin_colors { get; set; }
        public string Hair_colors { get; set; }
        public string Eye_colors { get; set; }
        public string Average_lifespan { get; set; }
        public string Homeworld { get; set; }
        public string Language { get; set; }
        public string[] People { get; set; }
        public string[] Films { get; set; }
    }

}
