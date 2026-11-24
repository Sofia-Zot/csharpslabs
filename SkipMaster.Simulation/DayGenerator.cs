using System;
using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

public sealed class DayGenerator(IRandomSource random) : IDayGenerator
{
    public (DayOutcome Outcome, YesterdayInfo NextYesterday) RunDay(
        int day,
        IReadOnlyList<Teacher> teachers,
        ISkipStrategy strategy,
        IStudentHistory history,
        YesterdayInfo yesterday)
    {
        bool[] attendancePlan = strategy.DecideDay(day, history);

        if (attendancePlan.Length != teachers.Count)
        {
            throw new ArgumentException(
                $"Стратегия вернула план на {attendancePlan.Length} пар, а преподавателей {teachers.Count}");
        }

        var outcomes = new SubjectOutcome[attendancePlan.Length];
        int pleasure = 0;
        bool exploded = false;

        for (int i = 0; i < attendancePlan.Length; i++)
        {
            Subject subject = (Subject)i;
            Teacher teacher = teachers[i];
            bool attended = attendancePlan[i];

            // спрашиваем в любом случае, иначе прогул и спросили не случится никогда
            bool asked = teacher.Ask(yesterday, random);

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
        var nextYesterday = new YesterdayInfo(outcomes);

        return (new DayOutcome(outcomes, exploded, pleasure), nextYesterday);
    }
}
