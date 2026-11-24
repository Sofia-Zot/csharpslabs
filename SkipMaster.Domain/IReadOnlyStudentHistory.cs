namespace SkipMaster.Domain;

/// <summary>
/// Что студент успел увидеть - только чтение.
/// </summary>
public interface IReadOnlyStudentHistory
{
    bool Attended(int day, Subject subject);

    bool? WasAsked(int day, Subject subject);
}
