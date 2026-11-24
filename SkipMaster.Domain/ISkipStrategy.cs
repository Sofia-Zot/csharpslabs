namespace SkipMaster.Domain;

/// <summary>
/// Стратегия прогулов студента.
/// </summary>
public interface ISkipStrategy
{
    string Name { get; }

    bool[] DecideDay(int day, IReadOnlyStudentHistory history);
}
