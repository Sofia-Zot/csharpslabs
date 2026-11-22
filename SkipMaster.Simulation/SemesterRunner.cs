using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

public sealed class SemesterRunner : ISemesterRunner
{
    private readonly IDayGenerator _dayGenerator;

    public SemesterRunner(IDayGenerator dayGenerator)
    {
        _dayGenerator = dayGenerator;
    }

    public SemesterResult Run(IReadOnlyList<Teacher> teachers, ISkipStrategy strategy, IStudentHistory history)
    {
        int totalPleasure = 0;

        // день 1 -> вчера никого не спрашивали
        var yesterday = YesterdayInfo.AllNotAsked();

        for (int day = 1; day <= SimulationDefaults.SemesterDays; day++)
        {
            (DayOutcome outcome, yesterday) = _dayGenerator.RunDay(day, teachers, strategy, history, yesterday);

            if (outcome.Exploded)
            {
                // вылет -> обнуляем накопленное и останавливаемся
                return new SemesterResult(0, day, exploded: true);
            }

            totalPleasure += outcome.Pleasure;
        }

        return new SemesterResult(totalPleasure, SimulationDefaults.SemesterDays, exploded: false);
    }
}
