using System;

namespace SkipMaster.Domain.Rules;

public sealed class XorSubjectsRule : IAskRule
{
    public XorSubjectsRule(Subject subjectA, Subject subjectB)
    {
        if (subjectA == subjectB)
        {
            throw new ArgumentException("Предметы A и Б должны различаться.", nameof(subjectB));
        }

        SubjectA = subjectA;
        SubjectB = subjectB;
    }

    public Subject SubjectA { get; }

    public Subject SubjectB { get; }

    // сработало ровно одно из двух -> спрашиваем
    public bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random)
    {
        return yesterday.WasAsked(SubjectA) ^ yesterday.WasAsked(SubjectB);
    }
}
