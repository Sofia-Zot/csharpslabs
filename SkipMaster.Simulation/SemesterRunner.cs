using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

public sealed class SemesterRunner : ISemesterRunner
{
    private readonly IDayGenerator _dayGenerator;
    private readonly IReadOnlyList<Teacher> _teachers;
    private readonly ISkipStrategy _strategy;
    private readonly IStudentHistory _history;
    private YesterdayInfo _yesterday;
    private int _daysCompleted;

    public SemesterRunner(
        IDayGenerator dayGenerator,
        TeacherProvider teacherProvider,
        ISkipStrategy strategy,
        IStudentHistory history)
    {
        _dayGenerator = dayGenerator;
        _teachers = teacherProvider.Teachers;
        _strategy = strategy;
        _history = history;

        // день 1 -> вчера никого не спрашивали
        _yesterday = YesterdayInfo.AllNotAsked();
        CurrentDay = 1;
    }

    public int CurrentDay { get; private set; }

    public int TotalPleasure { get; private set; }

    public bool Exploded { get; private set; }

    public double AveragePerDay => _daysCompleted == 0 ? 0 : TotalPleasure / (double)_daysCompleted;

    public DayOutcome? MakeStep()
    {
        if (Exploded)
        {
            return null;
        }

        (DayOutcome outcome, _yesterday) = _dayGenerator.RunDay(
            CurrentDay, _teachers, _strategy, _history, _yesterday);

        _daysCompleted++;

        if (outcome.Exploded)
        {
            // вылет -> обнуляем накопленное и останавливаемся
            TotalPleasure = 0;
            Exploded = true;
        }
        else
        {
            TotalPleasure += outcome.Pleasure;
        }

        CurrentDay++;
        return outcome;
    }
}
