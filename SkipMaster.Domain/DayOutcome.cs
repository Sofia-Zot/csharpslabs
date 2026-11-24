using System.Collections.Generic;

namespace SkipMaster.Domain;

public readonly record struct SubjectOutcome(Subject Subject, bool Attended, bool Asked);

/// <summary>
/// Итог дня по всем предметам.
/// </summary>
public sealed class DayOutcome(IReadOnlyList<SubjectOutcome> subjects, bool exploded, int pleasure)
{
    public IReadOnlyList<SubjectOutcome> Subjects => subjects;

    public bool Exploded => exploded;

    public int Pleasure => pleasure;
}
