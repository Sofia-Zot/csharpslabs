using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

public sealed class DayGenerator : IDayGenerator
{
    private readonly IRandomSource _random;

    public DayGenerator(IRandomSource random)
    {
        _random = random;
    }

    public (DayOutcome Outcome, YesterdayInfo NextYesterday) RunDay(
        int day,
        IReadOnlyList<Teacher> teachers,
        ISkipStrategy strategy,
        IStudentHistory history,
        YesterdayInfo yesterday)
    {
        bool[] attendancePlan = strategy.DecideDay(day, history);

        var outcomes = new SubjectOutcome[attendancePlan.Length];
        int pleasure = 0;
        bool exploded = false;

        for (int i = 0; i < attendancePlan.Length; i++)
        {
            Subject subject = (Subject)i;
            Teacher teacher = teachers[i];
            bool attended = attendancePlan[i];

            // спрашиваем в любом случае, иначе прогул и спросили не случится никогда
            bool asked = teacher.Ask(yesterday, _random);

            if (!attended)
            {
                pleasure += SimulationDefaults.PleasurePerSkip;

                if (asked)
                {
                    // прогулял и спросили -> вылет
                    exploded = true;
                }
            }

            history.Record(day, subject, attended, asked);
            outcomes[i] = new SubjectOutcome(subject, attended, asked);
        }

        // пирожок едим даже в день вылета
        pleasure += SimulationDefaults.PleasurePerPie;

        // сегодняшние опросы -> завтрашнее вчера
        var nextYesterday = new YesterdayInfo(
            outcomes[0].Asked,
            outcomes[1].Asked,
            outcomes[2].Asked,
            outcomes[3].Asked,
            outcomes[4].Asked,
            outcomes[5].Asked);

        return (new DayOutcome(outcomes, exploded, pleasure), nextYesterday);
    }
}
