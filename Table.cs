using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

public class Table
{
    private readonly IReadOnlyList<Fork> _forks;

    public Table(int seats, CancellationTokenSource cts)
    {
        SeatsCount = seats;
        _forks = new ReadOnlyCollection<Fork>((from n in Enumerable.Range(0, seats) select new Fork(n)).ToArray());
        CancellationTokenSource = cts;
    }

    public int SeatsCount { get; }

    public Random RandomNumberGenerator { get; } = new Random();

    public CancellationTokenSource CancellationTokenSource { get; }

    public Fork GetLeftFork(int seat)
    {
        int index = seat % SeatsCount;
        return _forks[index];
    }

    public Fork GetRightFork(int seat)
    {
        int index = (seat + 1) % SeatsCount;
        return _forks[index];
    }
}