using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class Program
    {
        private const int PHILOSOPHER_COUNT = 5;


        private static void Main(string[] args)
        {
            const int Seats = 5;
            const int MaxThinkingTime = 1000;
            const int MaxEatingTime = 2000;

            var cancellationToken = new CancellationTokenSource();
            var table = new Table(Seats, cancellationToken);

            var philosophers = new ReadOnlyCollection<global::Philosopher>(Enumerable.Range(0, Seats).Select(n => new global::Philosopher(n)).ToArray());

            var tasks = philosophers.Select(p => p.EatAsync(table, MaxThinkingTime, MaxEatingTime)).ToArray();

            Console.ReadLine();
            cancellationToken.Cancel(false);

            Task.WaitAll(tasks);
        }

        static void Main2(string[] args)
        {
            // init Forks
            var Forks = new List<Fork>(PHILOSOPHER_COUNT);

            for (int i = 0; i < PHILOSOPHER_COUNT; ++i)
            {
                Forks.Add(new Fork(i));
            }

            // init philosophers
            // first philosopher
            Task[] tasks = new Task[PHILOSOPHER_COUNT];
            tasks[0] = new Task(() => Philoshoper.Eat(Forks[0], Forks[PHILOSOPHER_COUNT - 1], 1, 1, PHILOSOPHER_COUNT));

            // other philosophers
            for (int i = 1; i < PHILOSOPHER_COUNT; ++i)
            {
                int ix = i;
                tasks[ix] = new Task(() => Philoshoper.Eat(Forks[ix - 1], Forks[ix], ix + 1, ix, ix + 1));
            }

            // dinner starts
            Console.WriteLine("Dinner starts!");

            Parallel.ForEach(tasks, t =>
            {
                t.Start();
            });

            // Wait until all philosophers finished
            Task.WaitAll(tasks);

            Console.WriteLine("Dinner done!");
            Console.ReadLine();
        }
    }

    public class Philoshoper
    {
        public static void Eat(Fork leftFork, Fork rightFork, int philosopherNumber, int leftForkNumber, int rightForkNumber)
        {
            while (true)
            {
                // philosopher thinks
                Random r = new Random();
                int thinkingTime = r.Next(0, 5000);
                Console.WriteLine($"Philosopher {philosopherNumber} thinks.");
                Thread.Sleep(thinkingTime);
                Console.WriteLine($"Philosopher {philosopherNumber} wants to take Forks.");

                Fork first = leftFork;
                Fork second = rightFork;
                int firstFork = leftForkNumber;
                int secondFork = rightForkNumber;

                bool isEven = philosopherNumber % 2 == 0;

                // switch forks to prevent deadlock
                if (isEven)
                {
                    first = rightFork;
                    firstFork = rightForkNumber;
                    second = leftFork;
                    secondFork = leftForkNumber;
                }

                lock (first)
                {
                    Console.WriteLine("Philosopher {0} picked {1} Fork.", philosopherNumber, firstFork);

                    // not needed because of deadlock prevention above
                    //lock (second)
                    //{

                    // philosopher eats
                    Console.WriteLine("Philosopher {0} picked {1} Fork.", philosopherNumber, secondFork);
                    Console.WriteLine("Philosopher {0} eats.", philosopherNumber);

                    int eating_time = r.Next(0, 10000);
                    Thread.Sleep(eating_time); // TODO: do not use Thread.Sleep() inside a task -> pauses ALL tasks on the thread (could be multiple)
                    Console.WriteLine("Philosopher {0} stops eating.", philosopherNumber);
                    //}
                    Console.WriteLine("Philosopher {0} released {1} Fork.", philosopherNumber, secondFork);

                }
                Console.WriteLine("Philosopher {0} released {1} Fork.", philosopherNumber, firstFork);
            }
        }
    }

    public class Fork
    {
        public Fork(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
