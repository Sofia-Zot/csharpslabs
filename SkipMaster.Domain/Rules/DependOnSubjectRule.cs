namespace SkipMaster.Domain.Rules;

public sealed class DependOnSubjectRule : IAskRule
{
    public DependOnSubjectRule(Subject subjectA)
    {
        SubjectA = subjectA;
    }

    public Subject SubjectA { get; }

    // вчера спросили по А -> спрашиваем и сегодня
    public bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random)
    {
        return yesterday.WasAsked(SubjectA);
    }
}
