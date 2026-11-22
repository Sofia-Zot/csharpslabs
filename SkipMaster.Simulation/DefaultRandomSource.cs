using System;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

// обертка над обычным random
public sealed class DefaultRandomSource : IRandomSource
{
    private readonly Random _random = new();

    public double NextDouble()
    {
        return _random.NextDouble();
    }

    public int Next(int maxExclusive)
    {
        return _random.Next(maxExclusive);
    }
}
