using System;
using System.Collections.Generic;

namespace FunctionalLib.Extensions
{
    public static class IEnumerableExtensions
    {
        // TODO:
        // - Extension method ForEach
        // - Functions as parameters
        // - Throw expressions
        // - nameof expressions

        // public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        // {
        //     list = list ?? throw new ArgumentNullException(nameof(list));
        //     action = action ?? throw new ArgumentNullException(nameof(action));

        //     foreach (var item in list) action(item);
        // }
        
        // TODO:
        // - Extension method TakeUntilIncluding
        // - Functions as parameters
        // - Local function
        // - Throw expressions
        // - nameof expressions
        // - Lazy listings with Iterators (yield)

        // public static IEnumerable<T> TakeUntilIncluding<T>(this IEnumerable<T> list, Func<T, bool> condition)
        // {
        //     list = list ?? throw new ArgumentNullException(nameof(list));
        //     condition = condition ?? throw new ArgumentNullException(nameof(condition));
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
