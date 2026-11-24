using System;
using SkipMaster.Domain;

namespace SkipMaster.Strategies;

/// <summary>
/// Прогуливает пару, если в последний посещённый день по ней не спрашивали; непосещённые предметы не рискует.
/// </summary>
public sealed class LastOutcomeStrategy : ISkipStrategy
{
    public string Name => "LastOutcome";

    public bool[] DecideDay(int day, IReadOnlyStudentHistory history)
    {
        var subjects = Enum.GetValues<Subject>();
        var plan = new bool[subjects.Length];

        for (int i = 0; i < subjects.Length; i++)
        {
            plan[i] = ShouldAttend(subjects[i], day, history);
        }

        return plan;
    }

    private static bool ShouldAttend(Subject subject, int day, IReadOnlyStudentHistory history)
    {
        for (int d = day - 1; d >= 1; d--)
        {
            if (history.Attended(d, subject))
            {
                return history.WasAsked(d, subject) == true;
            }
        }

        // предмет ещё не посещали -> идём
        return true;
    }
}
