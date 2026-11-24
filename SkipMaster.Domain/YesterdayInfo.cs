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
        _asked = new bool[outcomes.Count];

        for (int i = 0; i < outcomes.Count; i++)
        {
            _asked[i] = outcomes[i].Asked;
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
