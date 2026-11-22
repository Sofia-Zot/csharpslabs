using System;
using SkipMaster.Domain;

namespace SkipMaster.Strategies;

// стратегия будет потом
public sealed class AlwaysAttendStrategy : ISkipStrategy
{
    public string Name => "AlwaysAttend";

    public bool[] DecideDay(int day, IReadOnlyStudentHistory history)
    {
        int subjectsCount = Enum.GetValues<Subject>().Length;
        bool[] plan = new bool[subjectsCount];

        for (int i = 0; i < plan.Length; i++)
        {
            plan[i] = true;
        }

        return plan;
    }
}
