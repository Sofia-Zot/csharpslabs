using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

// прогон по шагам - по одному дню за вызов, пока не вылет
public interface ISemesterRunner
{
    // следующий учебный день; если был вылет - ничего не происходит
    DayOutcome? MakeStep();

    int CurrentDay { get; }

    int TotalPleasure { get; }

    double AveragePerDay { get; }

    bool Exploded { get; }
}
