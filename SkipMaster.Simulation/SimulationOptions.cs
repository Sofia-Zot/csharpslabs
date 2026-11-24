namespace SkipMaster.Simulation;

/// <summary>
/// Настройки симуляции - читаются из appsettings через di.
/// </summary>
public sealed class SimulationOptions
{
    public int DayDelayMilliseconds { get; set; } = 5000;
}
