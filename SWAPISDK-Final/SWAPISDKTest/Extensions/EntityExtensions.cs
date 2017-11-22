using FunctionalLib.Extensions;
using System;
using System.Collections.Generic;
using SWAPISDK.Entities;

namespace SWAPISDKTest.Extensions
{
    static class EntityExtensions
    {
        public static void Show<T>(this T entity) where T : Entity
        {
            switch(entity)
            {
                case Person person:
                    Console.WriteLine($"[{nameof(Person)}] {person.Name}");
                    break;
                case Planet planet:
                    Console.WriteLine($"[{nameof(Planet)}] {planet.Name}");
                    break;
                case Film film:
                    Console.WriteLine($"[{nameof(Film)}] {film.Title}");
                    break;
                case Species species:
                    Console.WriteLine($"[{nameof(Species)}] {species.Name}");
                    break;
                case Vehicle vehicle: 
                    Console.WriteLine($"[{nameof(Vehicle)}] {vehicle.Name}");
                    break;
                case Starship starship when starship.Name.Equals("Millennium Falcon"):
                    Console.WriteLine($"[{nameof(Starship)}] {starship.Name} --> my favourite starship ever!!!");
                    break;
                case Starship starship:
                    Console.WriteLine($"[{nameof(Starship)}] {starship.Name}");
                    break;   
                default:
                    throw new ArgumentException(nameof(entity));             
            }
        }
        
        public static void Show<T>(this IEnumerable<T> entities) where T : Entity
            => entities.ForEach(Show);
    }
}