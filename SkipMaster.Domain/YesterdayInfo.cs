using System;
using System.Collections.Generic;

namespace SkipMaster.Domain;

/// <summary>
/// Кто спрашивал вчера по каждому предмету - флаги на конец предыдущего дня.
/// </summary>
public readonly struct YesterdayInfo
{
    private readonly bool[] _asked;

    public YesterdayInfo(IReadOnlyList<SubjectOutcome> outcomes)
    {
        _asked = new bool[Enum.GetValues<Subject>().Length];

        foreach (SubjectOutcome outcome in outcomes)
        {
            _asked[(int)outcome.Subject] = outcome.Asked;
        }
    }

    private YesterdayInfo(bool[] asked)
    {
        _asked = asked;
    }

    public static YesterdayInfo AllNotAsked()
    {
        return new YesterdayInfo(new bool[Enum.GetValues<Subject>().Length]);
    }

    public bool WasAsked(Subject subject)
    {
        return _asked[(int)subject];
    }
}
