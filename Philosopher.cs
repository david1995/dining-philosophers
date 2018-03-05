using System.Threading.Tasks;

public class Philosopher
{
    public Philosopher(int index)
    {
        Index = index;
    }

    public int Index { get; }

    public async Task EatAsync(Table table, int maxThinkingTime, int maxEatingTime)
    {
        bool isEven = Index % 2 == 0;

        while (!table.CancellationTokenSource.IsCancellationRequested)
        {
            // think
            int thinkingTime = table.RandomNumberGenerator.Next(maxThinkingTime);
            await Task.Delay(thinkingTime);
            
            // take forks
            var firstFork = isEven ? table.GetRightFork(Index) : table.GetLeftFork(Index);
            var secondFork = isEven ? table.GetLeftFork(Index) : table.GetRightFork(Index);

            firstFork.TakeFork(this);
            secondFork.TakeFork(this);

            // eat
            int eatingTime = table.RandomNumberGenerator.Next(maxEatingTime);
            await Task.Delay(eatingTime);

            // put back forks
            secondFork.ReleaseFork(this);
            firstFork.ReleaseFork(this);
        }
    }
}