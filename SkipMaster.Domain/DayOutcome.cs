using System.Collections.Generic;

namespace SkipMaster.Domain;

public readonly record struct SubjectOutcome(Subject Subject, bool Attended, bool Asked);

// итог дня по всем предметам
public sealed class DayOutcome
{
    public DayOutcome(IReadOnlyList<SubjectOutcome> subjects, bool exploded, int pleasure)
    {
        Subjects = subjects;
        Exploded = exploded;
        Pleasure = pleasure;
    }

    public IReadOnlyList<SubjectOutcome> Subjects { get; }

    public bool Exploded { get; }

    public int Pleasure { get; }
}
