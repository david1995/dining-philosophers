using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace DiningPhilosophers
{
    class Program
    {
        private const int PHILOSOPHER_NUM = 5;
        private const int THINKING_TIME = 5000; //max
        private const int EATING_TIME = 10000; //max
        static readonly object locker = new object();

        static void Main(string[] args)
        {
            // init philosophers and chopsticks
            var philosophers = new List<Philosopher>(PHILOSOPHER_NUM);
            var chopsticks = new List<Chopstick>(PHILOSOPHER_NUM);

            Console.WriteLine("Dinner starts");

            for (int i = 0; i < PHILOSOPHER_NUM; i++)
            {
                philosophers.Add(new Philosopher(philosophers, i));
                startDinner(i);
            }

            // dinner done
            Console.WriteLine("Dinner is over!");
            Console.ReadLine();
        }

        public static void startDinner(int index)
        {
            Thread phil = new Thread(() => Console.WriteLine("phil" + index + "starts"));
            phil.Start();
            Random t = new Random();
            int think = t.Next(0, THINKING_TIME);
            Thread.Sleep(think);
            Console.WriteLine("phil" + index + "finished thinking");

            lock (locker)
            {
                Console.WriteLine("locked");
                //take fork[index]
                //printf('phil' + index + 'took first fork: index)
                //take fork[(index + 1) mod n]
                //printf('phil' + index + 'took second fork: index)
                //int t2 = random between 0 and eatingTime
                //sleep(t)
                //printf('phil' + index + ' is done eating')
                //putBackForks(index, (index + 1) mod n)
            }

            phil.Join();
        }

    }

    public class Philosopher
    {
        private readonly List<Philosopher> _allPhilosophers;
        private readonly int _i;

        public Philosopher(List<Philosopher> allPhilosophers, int index)
        {
            _allPhilosophers = allPhilosophers;
            _i = index;
            this.Name = string.Format("philosopher {0}", _i);
        }

        public string Name { get; private set; }
    }

    public class Chopstick
    {
        private readonly int _i;
        private bool _state;

        public Chopstick(int index, bool state)
        {
            _i = index;
            _state = state;

        }
    }
}