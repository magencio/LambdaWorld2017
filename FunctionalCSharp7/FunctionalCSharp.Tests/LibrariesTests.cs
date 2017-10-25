using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace FunctionalCSharp.Tests
{
    using LanguageExt;
    using LanguageExt.ClassInstances;
    using static LanguageExt.Prelude;

    [Collection("SystemCollectionsImmutable")]
    public class SystemCollectionsImmutableTests
    {
        [Fact]
        public void TestImmutableLists()
        {
            var list = ImmutableList.Create(1, 2, 3);

            list = Enumerable.Range(1, 3).ToImmutableList();

            var builder = ImmutableList.CreateBuilder<int>();
            builder.Add(1);
            builder.Add(2);
            builder.Add(3);
            list = builder.ToImmutable();

            var list2 = list.Add(4);

            Assert.Equal(new[] { 1, 2, 3 }, list);
            Assert.Equal(new[] { 1, 2, 3, 4 }, list2);
        }
    }

    [Collection("SystemInteractive")]
    public class SystemInteractiveTests
    {
        [Fact]
        public void TestInteractiveExtensions()
        {
            // Lazily invoke an action
            Console.WriteLine("Do:");
            var numbers = new[] { 30, 40, 20, 40 };
            var result = numbers.Do(Console.WriteLine);

            Console.WriteLine("Before Enumeration");
            result.ForEach(n => { }); // The action will be invoked when enumerating
            Console.WriteLine("After Enumeration");

            // Generate a sequence by repeating the source while the condition is true
            Console.WriteLine("DoWhile:");
            var then = DateTime.Now.Add(new TimeSpan(0, 0, 1));
            result = numbers.DoWhile(() => DateTime.Now < then);

            Console.WriteLine("Before Enumeration");
            result.ForEach(Console.WriteLine);
            Console.WriteLine("After Enumeration");

            // Generate a sequence of non-overlapping adjacent buffers over the source sequence
            var results = numbers.Buffer(3).ToArray();
            Assert.Equal(2, results.Count());
            Assert.Equal(new[] { 30, 40, 20 }, results[0].ToArray());
            Assert.Equal(new[] { 40 }, results[1].ToArray());

            // Apply an accumulator to generate a sequence of accumulated values
            result = numbers.Scan(0, (sum, num) => sum + num); // 0 is the seed

            Assert.Equal(new[] { 0, 30, 70, 90, 130 }, result.ToArray());
        }
    }

    [Collection("LanguageExtCore")]
    public class LanguageExtCoreTests
    {
        // List pattern matching
        public int Sum(IEnumerable<int> list) =>
            match(list,
                  () => 0,
                  (x, xs) => x + Sum(xs));

        [Fact]
        public void TestCSharpFunctionalLanguageExtensions()
        {
            // Option
            var optional = Some(123);
            var num = match(optional,
                            Some: v => v * 2,
                            None: () => 0);
            Assert.Equal(num, 246);

            // Monad transformers
            var list = List(Some(1), None, Some(2), None, Some(3));
            var presum = list.SumT<TInt, int>();
            Assert.Equal(6, presum);

            list = list.MapT(x => x * 2);
            var postsum = list.SumT<TInt, int>();
            Assert.Equal(12, postsum);

            // Lack of lambda and expression inference
            var add = fun((int x, int y) => x + y);
            Assert.Equal(7, add(4, 3));

            // Immutable lists
            var test = List(1, 2, 3, 4, 5).Map(x => x * 10).Filter(x => x > 20).Fold(0, (x, s) => s + x);
            Assert.Equal(120, test);

            // List pattern matching
            var sum = Sum(List(10, 20, 30, 40, 50));
            Assert.Equal(150, sum);
        }
    }
}