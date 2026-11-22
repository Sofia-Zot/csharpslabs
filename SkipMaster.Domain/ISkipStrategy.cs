namespace SkipMaster.Domain;

// стратегии (потом)
public interface ISkipStrategy
{
    string Name { get; }

    bool[] DecideDay(int day, IReadOnlyStudentHistory history);
}
