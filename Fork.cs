using System;
using System.Threading;

public class Fork
{
    private readonly object _locker = new object();
    private Philosopher _acquiredBy;

    public Fork(int index)
    {
        Index = index;
    }

    public int Index { get; }

    public void TakeFork(Philosopher acquisitor)
    {
        if (acquisitor is null)
        {
            throw new ArgumentNullException(nameof(acquisitor));
        }

        Monitor.Enter(_locker);
        _acquiredBy = acquisitor;
    }

    public void ReleaseFork(Philosopher acquiredBy)
    {
        if (acquiredBy is null)
        {
            throw new ArgumentNullException(nameof(acquiredBy));
        }

        if (_acquiredBy is null || _acquiredBy != acquiredBy)
        {
            throw new InvalidOperationException();
        }

        _acquiredBy = null;
        Monitor.Exit(_locker);
    }
}