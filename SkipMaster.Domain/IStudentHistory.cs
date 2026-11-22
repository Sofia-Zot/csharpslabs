namespace SkipMaster.Domain;

// пишет только симуляция
public interface IStudentHistory : IReadOnlyStudentHistory
{
    void Record(int day, Subject subject, bool attended, bool asked);
}
