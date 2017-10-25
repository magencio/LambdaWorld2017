using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace FunctionalCSharp.Tests
{
    using FunctionalCSharp.Extensions;
    using FunctionalCSharp.Types;
    using static FunctionalCSharp.Types.Option;
    using static FunctionalCSharp.Extensions.GeneralExtensions;
    using static FunctionalCSharp.Extensions.MaybeExtensions;
    using HelperFSharp;

    [Collection("PatternMatching")]
    public class PatternMatchingTests
    {
        public int GetInt(object o)
            => (o is int i || (o is string s && int.TryParse(s, out i))) ? i: throw new ArgumentException(nameof(o));

        [Fact]
        public void TestIsExpressionsWithPatterns()
        {
            int i1 = GetInt(5);
            int i2 = GetInt("23");

            Assert.Equal(5, i1);
            Assert.Equal(23, i2);
            Assert.Throws<ArgumentException>(() => GetInt("Alex"));
            Assert.Throws<ArgumentException>(() => GetInt(new {Name = "Alejandro", Age = 40}));
        }

        public class Circle
        {
            public double Radius { get; }

            public Circle(double radius) => Radius = radius;
        } 

        public class Rectangle 
        {
            public double Height { get; }
            public double Width { get; }

            public Rectangle(double height, double width)
            {
                Height = height;
                Width = width;
            }
        }

        public double CalculateArea(object shape)
        {
            switch(shape)
            {
                case Circle c:
                    return Math.PI * c.Radius * c.Radius;
                case Rectangle s when (s.Height == s.Width):
                    Console.WriteLine("This is a square!");
                    return s.Height * s.Height;
                case Rectangle r:
                    return r.Height * r.Width;
                case null:
                    throw new ArgumentNullException(nameof(shape));
                default:
                    throw new ArgumentException("Unknown shape", nameof(shape));
            }
        }

        [Fact]
        public void TestSwitchStatementWithPatterns()
        {
            var circleArea = CalculateArea(new Circle(3.0));
            var squareArea = CalculateArea(new Rectangle(2.0, 2.0));
            var rectangleArea = CalculateArea(new Rectangle(3.0, 2.0));
            
            Assert.Equal(Math.PI * 9.0, circleArea);
            Assert.Equal(4, squareArea);
            Assert.Equal(6, rectangleArea);
            Assert.Throws<ArgumentNullException>(() => CalculateArea(null));
            Assert.Throws<ArgumentException>(() => CalculateArea("some string"));
        }
    }

    [Collection("RefactoringIntoPureFunctions")]
    public class RefactoringIntoPureFunctionsTests
    {
        private string aMember = "StringOne";

        public void HyphenatedConcat(string appendStr) => aMember += '-' + appendStr;

        public void HyphenatedConcat(StringBuilder sb, string appendStr) => sb.Append('-' + appendStr);

        public string HyphenatedConcat(string s, string appendStr) => $"{s}-{appendStr}";

        [Fact]
        public void TestImpureAndPureFunctions()
        {
            // Impure
            HyphenatedConcat("StringTwo");
            Assert.Equal("StringOne-StringTwo", aMember);

            // Impure
            var sb1 = new StringBuilder("StringOne");
            HyphenatedConcat(sb1, "StringTwo");
            Assert.Equal("StringOne-StringTwo", sb1.ToString());

            // Pure
            var s1 = "StringOne";
            var s2 = HyphenatedConcat(s1, "StringTwo");
            Assert.Equal("StringOne", s1);
            Assert.Equal("StringOne-StringTwo", s2);
        }
    }

    [Collection("BuildingImmutableTypes")]
    public class BuildingImmutableTypesTests
    {
        public class ProductPile
        {
            public string ProductName { get; }

            public int Amount { get; }

            public decimal Price { get; }

            public ProductPile(string productName, int amount, decimal price)
            {
                productName.Require(p => !string.IsNullOrWhiteSpace(p));
                amount.Require(a => a >= 0);
                price.Require(p => p > 0);

                ProductName = productName;
                Amount = amount;
                Price = price;
            }

            public ProductPile SubtractOne()
                => new ProductPile(ProductName, Amount - 1, Price);
        }

        [Fact]
        public void TestImmutability()
        {
            Assert.Throws<ArgumentException>(() => new ProductPile(null, 3, 13));
        }

        [Fact]
        public void TestImmutabilityWithInvalidArgument()
        {
            var product = new ProductPile("Milk", 3, 13);
            Assert.Equal(3, product.Amount);

            var newProduct = product.SubtractOne();
            Assert.Equal(3, product.Amount);
            Assert.Equal(2, newProduct.Amount);
        }
    }

    [Collection("ImmutableTypesOptions")]
    public class ImmutableTypesOptionsTests
    {
        [Fact]
        public void TestOption()
        {
            var optInt1 = new Option<int>(10);
            var optInt2 = 10.ToOption();
            var optInt3 = Some(10);

            Assert.Equal(true, optInt1.IsSome);
            Assert.Equal(10, optInt1.Value);
            Assert.Equal(true, optInt1 == optInt2);
            Assert.Equal(true, optInt2 == optInt3);

            var optString1 = Some("test");
            var optString2 = Some<string>(null);

            Assert.Equal(true, optString1.IsSome);
            Assert.Equal("test", optString1.Value);
            Assert.Equal(true, optString2.IsSome);
            Assert.Equal(null, optString2.Value);

            optString1 = None;

            Assert.Equal(true, optString1.IsNone);
        }
    }

    [Collection("RecursionWithLambdas")]
    public class RecursionWithLambdasTests
    {
        public IEnumerable<int> InfiniteNumbers()
        {
            for (int i = 0; true; i++)
            {
                Console.WriteLine($"Number is {i}");
                yield return i;
            }
        }

        [Fact]
        public void TestRecursionWithLambdas()
        {
            var results = InfiniteNumbers().Take(10).Select(
                num =>
                {
                    Func<int, int> factorial = null;
                    factorial = n => n == 0 ? 1 : n * factorial(n - 1);
                    return factorial(num);
                });

            results.ForEach(result => Console.WriteLine($"Factorial is {result}"));

            Assert.Equal(new[] { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 }, results.ToList()); 
        }
    }

    [Collection("LazyEvaluation")]
    public class LazyEvaluationTests
    {
        public Lazy<int> LazySum(Lazy<int> a, Lazy<int> b)
        {
            Console.WriteLine("LazySum");
            return new Lazy<int>(() => { Console.WriteLine("a + b"); return a.Value + b.Value; });
        }

        [Fact]
        public void TestLazyEvaluation()
        {
            var a = new Lazy<int>(() => { Console.WriteLine("a"); return 10 * 2; });
            var b = new Lazy<int>(() => { Console.WriteLine("b"); return 5; });

            var result = LazySum(a, b);

            Console.WriteLine("Result:");
            Console.WriteLine(result.Value);

            Assert.Equal(25, result.Value);
        }
    }

    [Collection("PartialFunctions")]
    public class PartialFunctionsTests
    {
        public double Distance(double x, double y, double z) => Math.Sqrt(x * x + y * y + z * z);

        [Fact]
        public void TestPartialFunctions()
        {
            Func<double, double, double, double> distance3D = Distance;
            Func<double, double, double> distance2D = distance3D.SetDefaultArgument(0);
            var d1 = distance2D(3, 6);  // distance3D(3, 6, 0);
            var d2 = distance2D(3, 4);  // distance3D(3, 4, 0);
            var d3 = distance2D(1, 2);  // distance3D(1, 2, 0);

            Assert.Equal(6.7082039324993694, d1);
            Assert.Equal(5, d2);
            Assert.Equal(2.23606797749979, d3);
        }
    }

    [Collection("CurryFunctions")]
    public class CurryFunctionsTests
    {
        public double Distance(double x, double y, double z) => Math.Sqrt(x * x + y * y + z * z);

        [Fact]
        public void TestCurryFunction()
        {
            Func<double, double, double, double> distance3D = Distance;

            var curriedDistance = distance3D.Curry();

            var d = curriedDistance(3)(4)(12);

            Assert.Equal(13, d);
        }
    }

    [Collection("UncurryFunctions")]
    public class UncurryFunctionsTests
    {
        public double Distance(double x, double y, double z) => Math.Sqrt(x * x + y * y + z * z);

        [Fact]
        public void TestUnCurryFunction()
        {
            Func<double, double, double, double> distance3D = Distance;
            var curriedDistance = distance3D.Curry();

            Func<double, double, double, double> originalDistance = curriedDistance.UnCurry();

            Assert.Equal(13, curriedDistance(3)(4)(12));
            Assert.Equal(13, originalDistance(3, 4, 12));
        }
    }

    [Collection("Composition")]
    public class CompositionTests
    {
        public Func<X, Z> Compose<X, Y, Z>(Func<X, Y> f, Func<Y, Z> g)
        {
            return (x) => g(f(x));
        }

        [Fact]
        public void TestComposition()
        {
            Func<int, int> calcBwithA = a => a * 3;
            Func<int, int> calcCwithB = b => b + 27;

            var calcCWithA = Compose(calcBwithA, calcCwithB);
            Assert.Equal(39, calcCWithA(4));

            // or...

            calcCWithA = calcBwithA.Compose(calcCwithB);
            Assert.Equal(39, calcCWithA(4));
        }
    }

    [Collection("Map")]
    public class MapTests
    {
        [Fact]
        public void TestMap()
        {
            var people = new List<dynamic> {
                new {Name = "Harry", Age = 32},
                new {Name = "Anna", Age = 45},
                new {Name = "Willy", Age = 43},
                new {Name = "Rose", Age = 37}
            };

            people.Map(p => p.Name).ForEach(Console.WriteLine);

            Console.WriteLine("Same thing with LinQ:");

            people.Select(p => p.Name).ForEach(Console.WriteLine);
        }
    }

    [Collection("Filter")]
    public class FilterTests
    {
        [Fact]
        public void TestFilter()
        {
            var people = new List<dynamic> {
                new {Name = "Harry", Age = 32},
                new {Name = "Anna", Age = 45},
                new {Name = "Willy", Age = 43},
                new {Name = "Rose", Age = 37}
            };

            people.Filter(p => p.Age > 40).ForEach(Console.WriteLine);

            Console.WriteLine("Same thing with LinQ:");

            people.Where(p => p.Age > 40).ForEach(Console.WriteLine);
        }
    }

    [Collection("FoldLeft")]
    public class FoldLeftTests
    {
        [Fact]
        public void TestFoldLeft()
        {
            var sumOf1to10 = Enumerable.Range(1, 10).FoldLeft("0", (acc, value) => $"({acc}+{value})");
            Assert.Equal("((((((((((0+1)+2)+3)+4)+5)+6)+7)+8)+9)+10)", sumOf1to10);

            var people = new List<dynamic> {
                new {Name = "Harry", Age = 32},
                new {Name = "Anna", Age = 45},
                new {Name = "Willy", Age = 43},
                new {Name = "Rose", Age = 37}
            };

            var result = people.FoldLeft(
                (totalAge: 0, count: 0),
                (acc, value) => (acc.totalAge + value.Age, acc.count + 1));
            var averageAge = result.totalAge / result.count;
            Assert.Equal(39, averageAge);

            // Same thing with LinQ

            sumOf1to10 = Enumerable.Range(1, 10).Aggregate("0", (acc, value) => $"({acc}+{value})");
            Assert.Equal("((((((((((0+1)+2)+3)+4)+5)+6)+7)+8)+9)+10)", sumOf1to10);

            result = people.Aggregate(
                (totalAge: 0, count: 0),
                (acc, value) => (acc.totalAge + value.Age, acc.count + 1));
            averageAge = result.totalAge / result.count;
            Assert.Equal(39, averageAge);
        }
    }

    [Collection("FoldRight")]
    public class FoldRightTests
    {
        [Fact]
        public void TestFoldRight()
        {
            var sumOf1to10 = Enumerable.Range(1, 10).FoldRight("0", (value, acc) => $"({value}+{acc})");
            Assert.Equal("(1+(2+(3+(4+(5+(6+(7+(8+(9+(10+0))))))))))", sumOf1to10);

            var people = new List<dynamic> {
                new {Name = "Harry", Age = 32},
                new {Name = "Anna", Age = 45},
                new {Name = "Willy", Age = 43},
                new {Name = "Rose", Age = 37}
            };

            var result = people.FoldRight(
                (totalAge: 0, count: 0),
                (value, acc) => (value.Age + acc.totalAge, 1 + acc.count));
            var averageAge = result.totalAge / result.count;
            Assert.Equal(39, averageAge);

            // In LinQ, Aggregate is a left folding operator

            sumOf1to10 =
                Enumerable.Range(1, 10).
                Aggregate<int, Func<string, string>>(x => x, (f, value) => acc => f($"({value}+{acc})"))("0");
            Assert.Equal("(1+(2+(3+(4+(5+(6+(7+(8+(9+(10+0))))))))))", sumOf1to10);

            result =
                people.
                Aggregate<dynamic, Func<(int totalAge, int count), (int, int)>>(
                    x => x, 
                    (f, value) => acc => f((value.Age + acc.totalAge, 1 + acc.count)))((totalAge: 0, count: 0));
            averageAge = result.totalAge / result.count;
            Assert.Equal(39, averageAge);
        }
    }

    [Collection("Memoization")]
    public class MemoizationTests
    {
        [Fact]
        public void TestMemoization()
        {
            Func<long, long> fib = null;
            fib = n => n > 1 ? fib(n - 1) + fib(n - 2) : n;

            var res = MeasureTime(() => fib(40));

            Assert.Equal(102334155, res.result);
            Console.WriteLine($"Standard version took {res.time} milliseconds");

            fib = fib.Memoize();
            res = MeasureTime(() => fib(40));

            Assert.Equal(102334155, res.result);
            Console.WriteLine($"Memoized version took {res.time} milliseconds");
        }
    }

    [Collection("Monads")]
    public class MonadsTests
    {
        public Maybe<int> Add4(int x) => (x + 4).ToMaybe();

        public Maybe<int> MultBy2(int x) => (x * 2).ToMaybe();

        public Maybe<int> JustFail(int x) => new Nothing<int>();

        [Fact]
        public void TestMaybeMonad()
        {
            var result = 3.ToMaybe().Bind(Add4).Bind(MultBy2);
            Assert.Equal("14", result.ToString());

            result = 3.ToMaybe().Bind(Add4).Bind(JustFail).Bind(MultBy2);
            Assert.Equal("Nothing", result.ToString());
        }

        public Maybe<int> DoSomeDivision(int denominator) 
            => from a in 12.Div(denominator)
               from b in a.Div(2)
               select b;

        [Fact]
        public void TestMaybeMonadWithLinQ()
        {
            var result = from a in "Hello World!".ToMaybe()
                         from b in DoSomeDivision(2)
                         from c in (new DateTime(2016, 2, 24)).ToMaybe()
                         select $"{a} {b} {c.ToShortDateString()}";
            Assert.Equal("Hello World! 3 24/02/2016", result.ToString());

            result = from a in "Hello World!".ToMaybe()
                     from b in DoSomeDivision(0)
                     from c in (new DateTime(2016, 2, 24)).ToMaybe()
                     select $"{a} {b} {c.ToShortDateString()}";
            Assert.Equal("Nothing", result.ToString());
        }
    }

    [Collection("CSharpAndFSharpInterop")]
    public class CSharpAndFSharpInteropTests
    {
        [Fact]
        public void TestDiscriminatedUnionsWithPatternMatching()
        {
            var shapes = new List<Shape>
            {
                Shape.NewCircle(15.0),
                Shape.NewSquare(10.0),
                Shape.NewRectangle(5.0, 10.0),
                Shape.NewEquilateralTriangle(10.0)
            };

            var areas = shapes.Select(s => s.Area()).ToArray();

            Assert.Equal(new[] { 706.85834715, 100, 50, 43.301270189221924 }, areas);
        }
    }

    [Collection("ImperativeVSFunctionalStyle")]
    public class ImperativeVSFunctionalStyleTests
    {
        private void NonFunctionalExample(string text)
        {
            using (StringReader rdr = new StringReader(text))
            {
                string contents = rdr.ReadToEnd();
                string[] words = contents.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].Trim();
                }

                Dictionary<string, int> d = new Dictionary<string, int>();

                foreach (string word in words)
                {
                    if (d.ContainsKey(word))
                    {
                        d[word]++;
                    }
                    else
                    {
                        d.Add(word, 1);
                    }
                }

                foreach (KeyValuePair<string, int> kvp in d)
                {
                    Console.WriteLine(string.Format("({0}, {1})", kvp.Key, kvp.Value.ToString()));
                }
            }
        }

        private void FunctionalExample(string text)
            => new StringReader(text).Use(stream => stream
                .ReadToEnd()
                .Split(' ')
                .Select(str => str.Trim())
                .GroupBy(str => str)
                .Select(group => $"({group.Key}, {group.Count()})")
                .ForEach(Console.WriteLine));

        [Fact]
        public void Test()
        {
            Console.WriteLine("NON FUNCTIONAL SAMPLE:");
            NonFunctionalExample("En un lugar de la mancha de cuyo nombre no puedo acordarme");

            Console.WriteLine("FUNCTIONAL SAMPLE:");
            FunctionalExample("En un lugar de la mancha de cuyo nombre no puedo acordarme");
        }
    }
}