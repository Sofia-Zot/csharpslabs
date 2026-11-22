using SkipMaster.Domain;

namespace SkipMaster.Simulation;

// прогон семестра по шагам - по одному дню за вызов
public interface ISemesterRunner
{
    // следующий учебный день
    // если семестр кончился или студент вылетел - ничего не происходит
    DayOutcome? MakeStep();

    int CurrentDay { get; }

    int TotalPleasure { get; }

    double AveragePerDay { get; }

    bool Exploded { get; }

    bool SemesterFinished { get; }
}
