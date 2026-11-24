using System;

namespace SkipMaster.Domain.Rules;

/// <summary>
/// Спросить, если вчера спросили ровно по одному из двух предметов.
/// </summary>
public sealed class XorSubjectsRule : IAskRule
{
    public XorSubjectsRule(Subject subjectA, Subject subjectB)
    {
        if (subjectA == subjectB)
        {
            throw new ArgumentException("Предметы А и Б должны различаться.", nameof(subjectB));
        }

        SubjectA = subjectA;
        SubjectB = subjectB;
    }

    public Subject SubjectA { get; }

    public Subject SubjectB { get; }

    public bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random)
    {
        return yesterday.WasAsked(SubjectA) ^ yesterday.WasAsked(SubjectB);
    }
}
