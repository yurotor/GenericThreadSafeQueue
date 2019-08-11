using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericThreadSafeQueue
{
    class Program
    {
        static GenericQueue genericQueue = new GenericQueue();
        static void Main(string[] args)
        {
            GetA();
            GetB();

            while (true)
            {
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                while (genericQueue.TryDequeue<A>(out var a))
                {
                    Console.WriteLine($"A {a.ID}");
                }
                while (genericQueue.TryDequeue<B>(out var b))
                {
                    Console.WriteLine($"B {b.ID}");
                }
               
            }

        }

        public static void GetA()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    genericQueue.Enqueue(new A { ID = i });
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }
            });
        }

        public static void GetB()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    genericQueue.Enqueue(new B { ID = i });
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }
            });
        }
    }

    class A
    {
        public int ID { get; set; }
    }

    class B
    {
        public int ID { get; set; }
    }
}
