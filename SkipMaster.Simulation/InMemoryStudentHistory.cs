using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

/// <summary>
/// Пока в памяти (в лабе 4 переедет в базу).
/// </summary>
public sealed class InMemoryStudentHistory : IStudentHistory
{
    private readonly Dictionary<(int Day, Subject Subject), DayRecord> _records = new();

    public bool Attended(int day, Subject subject)
    {
        return _records.TryGetValue((day, subject), out DayRecord entry) && entry.Attended;
    }

    public bool? WasAsked(int day, Subject subject)
    {
        // исход опроса виден только на посещённых парах
        if (_records.TryGetValue((day, subject), out DayRecord entry) && entry.Attended)
        {
            return entry.Asked;
        }

        return null;
    }

    public void Record(int day, Subject subject, bool attended, bool asked)
    {
        _records[(day, subject)] = new DayRecord(attended, asked);
    }

    private readonly record struct DayRecord(bool Attended, bool Asked);
}
