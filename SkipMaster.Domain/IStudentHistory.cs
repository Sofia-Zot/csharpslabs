namespace SkipMaster.Domain;

/// <summary>
/// Пишет только симуляция.
/// </summary>
public interface IStudentHistory : IReadOnlyStudentHistory
{
    void Record(int day, Subject subject, bool attended, bool asked);
}
