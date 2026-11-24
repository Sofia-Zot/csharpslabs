namespace SkipMaster.Domain;

/// <summary>
/// Правило внутри, снаружи видно только Ask.
/// </summary>
public sealed class Teacher(Subject subject, IAskRule rule)
{
    public Subject Subject { get; } = subject;

    public bool Ask(YesterdayInfo yesterday, IRandomSource random)
    {
        return rule.ShouldAsk(yesterday, random);
    }
}
