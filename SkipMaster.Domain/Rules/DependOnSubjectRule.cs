namespace SkipMaster.Domain.Rules;

/// <summary>
/// Спросить, если вчера спросили по предмету А, иначе нет.
/// </summary>
public sealed class DependOnSubjectRule(Subject subjectA) : IAskRule
{
    public Subject SubjectA { get; } = subjectA;

    public bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random)
    {
        return yesterday.WasAsked(SubjectA);
    }
}
