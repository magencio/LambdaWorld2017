using System;
using System.Collections.Generic;

namespace FunctionalLib.Extensions
{
    public static class IEnumerableExtensions
    {
        // TODO:
        // - Extension method ForEach
        // - Functions as parameters
        // - nameof expressions

        // public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        // {
        //     if (list == null) throw new ArgumentNullException(nameof(list));
        //     if (action == null) throw new ArgumentNullException(nameof(action));

        //     foreach (var item in list) action(item);
        // }
        
        // TODO:
        // - Extension method TakeUntilIncluding
        // - Functions as parameters
        // - Local function
        // - nameof expressions
        // - Lazy listings with Iterators (yield)

        // public static IEnumerable<T> TakeUntilIncluding<T>(this IEnumerable<T> list, Func<T, bool> condition)
        // {
        //     if (list == null) throw new ArgumentNullException(nameof(list));
        //     if (condition == null) throw new ArgumentNullException(nameof(condition));
        //     return Iterator();

        //     IEnumerable<T> Iterator()
        //     {
        //         foreach (var item in list)
        //         {
        //             yield return item;
        //             if (condition(item)) yield break;
        //         }
        //     }
        // }
    }
}
