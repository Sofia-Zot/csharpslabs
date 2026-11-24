using SkipMaster.Domain;

namespace SkipMaster.Simulation;

/// <summary>
/// Прогон по шагам - по одному дню за вызов, пока не вылет.
/// </summary>
public interface ISemesterRunner
{
    // следующий учебный день; если был вылет - ничего не происходит
    DayOutcome? MakeStep();

    int CurrentDay { get; }

    int TotalPleasure { get; }

    double AveragePerDay { get; }

    bool Exploded { get; }
}
