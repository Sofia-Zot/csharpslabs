namespace SkipMaster.Simulation;

// настройки симуляции - читаются из appsettings через di
public sealed class SimulationOptions
{
    public int DayDelayMilliseconds { get; set; } = 5000;

    public int SemesterDays { get; set; } = 100;
}
