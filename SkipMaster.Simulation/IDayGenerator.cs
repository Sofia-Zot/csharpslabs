using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

// один учебный день целиком
public interface IDayGenerator
{
    (DayOutcome Outcome, YesterdayInfo NextYesterday) RunDay(
        int day,
        IReadOnlyList<Teacher> teachers,
        ISkipStrategy strategy,
        IStudentHistory history,
        YesterdayInfo yesterday);
}
