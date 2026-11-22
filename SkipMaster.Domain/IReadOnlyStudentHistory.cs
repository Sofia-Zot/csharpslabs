namespace SkipMaster.Domain;

// что студент успел увидеть - только чтение
public interface IReadOnlyStudentHistory
{
    bool Attended(int day, Subject subject);

    bool? WasAsked(int day, Subject subject);
}
