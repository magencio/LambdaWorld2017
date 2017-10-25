using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalCSharp.Tests
{
    using FunctionalCSharp.Extensions;
    using static FunctionalCSharp.Extensions.GeneralExtensions;

    // First-class Functions in C#
    [Collection("FunctionTypes")]
    public class FunctionTypesTests
    {
        // Strongly typed delegate
        public delegate double MyFunction(double x);

        [Fact]
        public void TestStronglyTypedDelegate()
        {
            MyFunction f = Math.Sin;
            double y = f(Math.PI / 2);

            Assert.Equal(1, y);

            f = Math.Exp;
            y = f(1);

            Assert.Equal(Math.E, y);
        }

        [Fact]
        public void TestGenericFunctionTypes()
        {
            // Generic function type
            Func<double, double> f = Math.Sin;
            double y = f(Math.PI / 2);

            Assert.Equal(1, y);

            f = Math.Exp;
            y = f(1);

            Assert.Equal(Math.E, y);

            // Predicate type
            Predicate<string> isEmptyString = string.IsNullOrEmpty;
            bool isEmpty = isEmptyString("Test");

            Assert.Equal(false, isEmpty);

            // Action type
            Action<string> println = Console.WriteLine;
            println("Test");
        }
    }

    [Collection("FunctionValues")]
    public class FunctionValuesTests
    {
        [Fact]
        public void TestAssignValuesToFunctions()
        {
            // Point to existing method by name
            Func<double, double> f1 = Math.Sin;
            Func<double, double> f2 = Math.Exp;
            double y = f1(Math.PI / 2) + f2(1);

            Assert.Equal(1 + Math.E, y);

            // Pass function to another function
            f2 = f1;
            y = f2(Math.PI / 2);

            Assert.Equal(1, y);
        }

        [Fact]
        public void TestAnonymousFunctions()
        {
            // Anonymous delegate
            Func<double, double> f = delegate (double x) { return 3 * x + 1; };
            double y = f(4);

            Assert.Equal(13, y);

            // Lambda Expression
            f = x => 3 * x + 1;
            y = f(5);

            Assert.Equal(16, y);
        }
    }

    [Collection("LambdaExpressions")]
    public class LambdaExpressionsTests
    {
        [Fact]
        public void TestExpressionLambdas()
        {
            Func<int> f1 = delegate () { return 3; };
            f1 = () => 3;

            Func<DateTime> f2 = delegate () { return DateTime.Now; };
            f2 = () => DateTime.Now;

            Func<int, int> f3 = delegate (int x) { return x + 1; };
            f3 = x => x + 1;

            Func<int, double> f4 = delegate (int x) { return Math.Log(x + 1) - 1; };
            f4 = (x) => Math.Log(x + 1) - 1;

            Func<int, int, int> f5 = delegate (int x, int y) { return x + y; };
            f5 = (x, y) => x + y;

            Func<string, string, string> f6 = delegate (string x, string y) { return $"{x} {y}"; };
            f6 = (x, y) => $"{x} {y}";
        }

        [Fact]
        public void TestStatementLambda()
        {
            Action<string> myDel = n =>
                {
                    string s = $"{n} World";
                    Console.WriteLine(s);
                };

            myDel("Hello");
        }
    }

    [Collection("FunctionsAsParameters")]
    public class FunctionsAsParametersTests
    {
        public void Operate(int x, int y, Func<int, int, int> operation)
        {
            Console.WriteLine(operation(x, y));
        }

        [Fact]
        public void TestFunctionsAsParameters()
        {
            Operate(5, 6, (x, y) => x + y);
            Operate(5, 6, (x, y) => x * y);
        }
    }

    [Collection("FunctionArithmetic")]
    public class FunctionArithmeticTests
    {
        public static void Hello(string s)
        {
            Console.WriteLine($"Hello, {s}!");
        }

        public static void Goodbye(string s)
        {
            Console.WriteLine($"  Goodbye, {s}!");
        }

        [Fact]
        public void TestFuntionArithmetic()
        {
            Action<string> action = Console.WriteLine;
            Action<string> hello = Hello;
            Action<string> goodbye = Goodbye;
            action += Hello;
            action += (x) => Console.WriteLine($"  Greating {x} from lambda expression");
            action("First");

            action -= hello;
            action("Second");

            action = Console.WriteLine + goodbye
                    + delegate (string x)
                    {
                        Console.WriteLine($"  Greating {x} from delegate");
                    };
            action("Third");

            (action - Goodbye)("Fourth");

            action.GetInvocationList().ToList().ForEach(del => Console.WriteLine(del.Method.Name));
        }
    }

    [Collection("HigherOrderFunctions")]
    public class HigherOrderFunctionsTests
    {
        public Func<X, Z> Compose<X, Y, Z>(Func<X, Y> f, Func<Y, Z> g)
        {
            return (x) => g(f(x));
        }

        [Fact]
        public void TestHigherOrderFunction()
        {
            Func<double, double> sin = Math.Sin;
            Func<double, double> exp = Math.Exp;
            Func<double, double> expSin = Compose(sin, exp);
            double y = expSin(3);

            Assert.Equal(1.151562836514535, y);
        }
    }

    [Collection("TypeInferenceAndAnonymousTypes")]
    public class TypeInferenceAndAnonymousTypesTests
    {
        [Fact]
        public void TestTypeInference()
        {
            var v = new { Amount = 108, Message = "Hello" };

            Assert.Equal(108, v.Amount);
            Assert.Equal("Hello", v.Message);
        }
    }

    [Collection("ExpressionBodiedMembers")]
    public class ExpressionBodiedMembersTests
    {
        public class SomeClass
        {
            // Constructor
            // public SomeClass(string name)
            // {
            //     if (name != null)
            //     {
            //         throw new ArgumentNullException("name");
            //     }
            //     this.name = name;
            // }
            public SomeClass(string name) => this.name = name ?? throw new ArgumentNullException(nameof(name));

            // Property get and set
            private string name;
            // public string Name
            // {
            //     get 
            //     {
            //         return name;
            //     }
            //     set                
            //     {
            //         name = value;
            //     }
            // }
            public string Name
            {
                 get => name;
                 set => name = value;   
            }

            // Read-only property
            // public string DefaultName
            // {
            //     get 
            //     {
            //         return "Alejandro Campos Magencio";
            //     }
            // }
            public string DefaultName => "Alejandro Campos Magencio";

            // Indexer
            private string[] types = { "Type 1", "Type 2", "Type 3" }; 
            // public string this[int i]
            // {
            //     get 
            //     { 
            //         return types[i];
            //     }
            //     set 
            //     {
            //         types[i] = value;
            //     }
            // }
            public string this[int i]
            {
                get => types[i];
                set => types[i] = value;
            }

            // Method
            // public override string ToString() 
            // {
            //    return $"Name: {name}";
            // }            
            public override string ToString() => $"Name: {name}";

            // Finalizer
            // ~SomeClass()
            // { 
            //     Console.WriteLine("Finalizer is running");
            // }
            ~SomeClass() => Console.WriteLine("Finalizer is running");
        }

        [Fact]
        public void TestExpressionBodiedMembers()
        {
            var obj = new SomeClass("Some name");

            Assert.Equal("Some name", obj.Name);
            Assert.Equal("Alejandro Campos Magencio", obj.DefaultName);
            Assert.Equal("Name: Some name", obj.ToString());
            Assert.Equal("Type 3", obj[2]);
        }
    }

    [Collection("ImmutableTypesTuples")]
    public class ImmutableTypesTuplesTests
    {
        private Random rnd = new Random();

        public (double, double) RandomPrice(double max)
            => (Math.Round(rnd.NextDouble() * max, 1), Math.Round(rnd.NextDouble() * max, 1));

        public (double, double) Normalize((double, double) price)
            => price.Item1 < price.Item2 ? price : (price.Item2, price.Item1);

        [Fact]
        public void TestTuples()
        {
            (double minPrice, double maxPrice) = Normalize(RandomPrice(100.0));

            Assert.Equal(true, minPrice <= maxPrice);
        }

        public class Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y) { X = x; Y = y; }

            public void Deconstruct(out int x, out int y) { x = X; y = Y; }
        }

        [Fact]
        public void TestDeconstructor()
        {
            var point = new Point(3, 4);
            var (myX, myY) = point;

            Assert.Equal(3, myX);
            Assert.Equal(4, myY);
        }
    }
    
    [Collection("Closures")]
    public class ClosuresTests
    {
        public (Action, Action, Action) CreateBoundFunctions()
        {
            int val = 0;

            Action increment = () => val++;
            Action decrement = () => val--;
            Action print = () => Console.WriteLine($"val = {val}");

            return (increment, decrement, print);
        }

        [Fact]
        public void TestBoundFunctions()
        {
            var (increment, decrement, print) = CreateBoundFunctions();

            increment();
            print();
            increment();
            print();
            increment();
            print();
            decrement();
            print();
        }
    }

    [Collection("ExpressionTrees")]
    public class ExpressionTreesTests
    {
        [Fact]
        public void TestExpressionTree()
        {
            // Create
            Expression<Func<int, bool>> exprTree = num => num < 5;

            // Decompose
            ParameterExpression param = exprTree.Parameters[0];
            BinaryExpression operation = (BinaryExpression)exprTree.Body;
            ParameterExpression left = (ParameterExpression)operation.Left;
            ConstantExpression right = (ConstantExpression)operation.Right;

            Assert.Equal("num => num LessThan 5", $"{param.Name} => {left.Name} {operation.NodeType} {right.Value}");

            // Compile
            Func<int, bool> result = exprTree.Compile();

            // Invoke
            Assert.Equal(true, result(4));
        }
    }

/*
    // Note: Delegate.BeginInvoke is not supported by .NET Core 2.0

    [Collection("AsyncFunctions")]
    public class AsyncFunctionsTests
    {
        public int SlowSum(int x, int y)
        {
            Thread.Sleep(10000);
            return x + y;
        }

        [Fact]
        public void TestSlowFunction()
        {
            Func<int, int, int> f = SlowSum;

            // Start execution
            IAsyncResult async = f.BeginInvoke(5, 3, null, null);

            int sum;

            // Check is function completed
            if (async.IsCompleted)
            {
                sum = f.EndInvoke(async);
            }

            //Finally demand result
            sum = f.EndInvoke(async);

            Assert.Equal(8, sum);
        }
    }
 */

    [Collection("TaskParallelLibrary")]
    public class TaskParallelLibraryTests
    {
        public int SlowSum(int x, int y)
        {
            Thread.Sleep(10000);
            return x + y;
        }

        [Fact]
        public void TestLambdasInParallel()
        {
            Console.WriteLine("Let's go!");

            var task1 = Task.Run(() => SlowSum(3, 4));
            var task2 = Task.Run(() => SlowSum(5, 6));

            Console.WriteLine("Doing something else in the meantime");

            Console.WriteLine($"Result 1 = {task1.Result}, Result 2 = {task2.Result}");

            Assert.Equal(7, task1.Result);
            Assert.Equal(11, task2.Result);
        }
    }   

    [Collection("AsyncLambdas")]
    public class AsyncLambdasTests
    {
        public async Task<string> ExampleMethodAsync()
        {
            // The following line simulates a task-returning asynchronous process.
            await Task.Delay(1000);
            return "Ok";
        }

        [Fact]
        public async Task TestExampleMethodAsync()
        {
            string result = await ExampleMethodAsync();

            Assert.Equal("Ok", result);
        }

        [Fact]
        public async Task TestExampleMethodWithLambdaAsync()
        {
            Func<Task<string>> f = async () => await ExampleMethodAsync();

            string result = await f();

            Assert.Equal("Ok", result);
        }
    }

    [Collection("Generics")]
    public class GenericsTests
    {
        public class GenericList<T>
        {
            void Add(T input) { }
        }

        public class ExampleClass { }

        [Fact]
        public void TestGenericType()
        {
            var list1 = new GenericList<int>();
            var list2 = new GenericList<string>();
            var list3 = new GenericList<ExampleClass>();
        }
    }

    [Collection("GenericMethods")]
    public class GenericMethodsTests
    {
        public int Count<T>(T[] arr, Predicate<T> condition)
        {
            int counter = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (condition(arr[i]))
                {
                    counter++;
                }
            }
            return counter;
        }

        [Fact]
        public void TestCount()
        {
            var words = new string[] { "Title", "Some Long Tile", "" };

            var numberOfBooksWithLongNames = Count(words, word => word.Length > 10);
            var numberOfEmptyBookTitles = Count(words, string.IsNullOrEmpty);

            var numbers = new int[] { 3, 4, -3, 0, -1 };

            var numberOfNegativeNumbers = Count(numbers, x => x < 0);

            Assert.Equal(1, numberOfBooksWithLongNames);
            Assert.Equal(1, numberOfEmptyBookTitles);
            Assert.Equal(2, numberOfNegativeNumbers);
        }
    }

    [Collection("GenericDelegates")]
    public class GenericDelegatesTests
    {
        public delegate void Del<T>(T item);

        public void Notify(int i) { }

        [Fact]
        public void TestGenericDelegate()
        {
            Del<int> m2 = Notify;
        }
    }

    [Collection("ExtensionMethods")]
    public class ExtensionMethodsTests
    {
        public class SomeClass : IDisposable
        {
            public void Dispose() => Console.WriteLine("Object disposed!");

            public void DoAction() => Console.WriteLine("Done!");
        }

        [Fact]
        public void TestGenericExtensionMethod()
        {
            //using (var obj = new SomeClass())
            //{
            //    obj.DoAction();
            //}

            var obj = new SomeClass();

            obj.Use(x => x.DoAction());
        }
    }

    [Collection("CovarianceAndContravariance")]
    public class CovarianceAndContravarianceTests
    {
        public class Person { }
        public class Employee : Person { }

        public Employee FindByTitle(string title) => new Employee();

        [Fact]
        public void TestCovariance()
        {
            Func<string, Employee> findEmployee = FindByTitle;

            Func<string, Person> findPerson = FindByTitle;

            findPerson = findEmployee;
        }

        public void AddToContacts(Person person) { }

        [Fact]
        public void TestContravariance()
        {
            Action<Person> addPersonToContacts = AddToContacts;

            Action<Employee> addEmployeeToContacts = AddToContacts;

            addEmployeeToContacts = addPersonToContacts;
        }

        // Invariant generic delegate
        public delegate T DInvariant<T>();

        // Covariant generic delegate
        public delegate R DCovariant<out R>();

        // Contravariant generic delegate
        public delegate void DContravariant<in A>(A a);

        // Variant generic delegate
        public delegate R DVariant<in A, out R>(A a);

        [Fact]
        public void TestVariance()
        {
            DInvariant<string> dinString = () => "";
            //DInvariant<object> dinObject = dinString;

            DCovariant<string> dcoString = () => " ";
            DCovariant<object> dcoObject = dcoString;

            DContravariant<object> dcontraObject = (a) => Console.WriteLine(a);
            DContravariant<string> dcontraString = dcontraObject;

            DVariant<object, string> dObjectString = (a) => $"{a} ";
            DVariant<object, object> dObjectObject = dObjectString;
            DVariant<string, string> dStringString = dObjectString;
            DVariant<string, object> dStringObject = dObjectString;
        }
    }

    [Collection("Dynamics")]
    public class DynamicsTests
    {
        [Fact]
        public void TestDynamics()
        {
            //Func<object, object> doubleIt = p => p + p;
            Func<dynamic, dynamic> doubleIt = p => p + p;

            Assert.Equal(4, doubleIt(2));
            Assert.Equal("22", doubleIt("2"));
        }
    }

    [Collection("LINQ")]
    public class LINQTests
    {
        [Fact]
        public void TestLinq()
        {
            var scores = new int[] { 60, 92, 81, 97 };

            var scoreQuery =
                from score in scores
                where score > 80
                orderby score
                select score;

            foreach (var i in scoreQuery)
            {
                Console.Write($"{i} ");
            }

            Assert.Equal(new[]{ 81, 92, 97 }, scoreQuery.ToList());
        }

        [Fact]
        public void TestLinqWithLambdas()
        {
            var scores = new int[] { 60, 92, 81, 97 };

            var scoreQuery = scores.Where(score => score > 80).OrderBy(score => score);

            foreach (var i in scoreQuery)
            {
                Console.Write($"{i} ");
            }

            Assert.Equal(new[] { 81, 92, 97 }, scoreQuery.ToList());
        }
    }

    [Collection("ParallelLINQ")]
    public class ParallelLINQTests
    {
        [Fact]
        public void TestPLINQ()
        {
            var nums = Enumerable.Range(10, 10000);
            var query = from num in nums.AsParallel()//.AsOrdered()
                        where num % 10 == 0
                        select num;

            var concurrentBag = new ConcurrentBag<string>();
            query.ForAll(e => concurrentBag.Add($"Value = {e}"));

            Assert.Equal(1000, concurrentBag.Count);
        }

        [Fact]
        public void TestPLINQWithLambdas()
        {
            var nums = Enumerable.Range(10, 10000);
            var query = nums.AsParallel()/*.AsOrdered()*/.Where(num => num % 10 == 0);

            var concurrentBag = new ConcurrentBag<string>();
            query.ForAll(e => concurrentBag.Add($"Value = {e}"));

            Assert.Equal(1000, concurrentBag.Count);
        }
    }

    [Collection("LazyListingsWithIterators")]
    public class LazyListingsWithIteratorsTests
    {
        private Random rnd = new Random();

        public IEnumerable<int> MyEndlessRandomNumberGenerator()
        {
            while (true)
            {
                yield return rnd.Next();
            }
        }

        [Fact]
        public void TestIterator()
        {
            MyEndlessRandomNumberGenerator().Take(5).ForEach(Console.WriteLine);
        }
    }

    [Collection("LocalFunctions")]
    public class LocalFunctionsTests
    {
        public IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> filter)
        {
            if (source == null) { throw new ArgumentException(nameof(source)); }
            if (filter == null) { throw new ArgumentException(nameof(filter)); }
            return Iterator();

            IEnumerable<T> Iterator()
            {
                foreach (var element in source)
                {
                    if (filter(element)) { yield return element; }
                }
            }
        }
    }
}
