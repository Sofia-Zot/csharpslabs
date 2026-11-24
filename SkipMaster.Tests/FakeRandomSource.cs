using System;
using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Tests;

/// <summary>
/// Заданные заранее значения -> прогон детерминирован, лишний запрос к рандому сразу виден.
/// </summary>
public sealed class FakeRandomSource : IRandomSource
{
    private readonly Queue<double> _doubles;
    private readonly Queue<int> _ints;

    private FakeRandomSource(IEnumerable<double> doubles, IEnumerable<int> ints)
    {
        _doubles = new Queue<double>(doubles);
        _ints = new Queue<int>(ints);
    }

    public static FakeRandomSource WithDoubles(params double[] values)
    {
        return new FakeRandomSource(values, Array.Empty<int>());
    }

    public static FakeRandomSource WithInts(params int[] values)
    {
        return new FakeRandomSource(Array.Empty<double>(), values);
    }

    public double NextDouble()
    {
        if (_doubles.Count == 0)
        {
            throw new InvalidOperationException("Фейковому источнику рандома не хватило double");
        }

        return _doubles.Dequeue();
    }

    public int Next(int maxExclusive)
    {
        if (_ints.Count == 0)
        {
            throw new InvalidOperationException("Фейковому источнику рандома не хватило int");
        }

        return _ints.Dequeue();
    }
}
