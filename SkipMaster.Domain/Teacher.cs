namespace SkipMaster.Domain;

// правило внутри, снаружи видно только Ask
public sealed class Teacher
{
    private readonly IAskRule _rule;

    public Subject Subject { get; }

    public Teacher(Subject subject, IAskRule rule)
    {
        Subject = subject;
        _rule = rule;
    }

    public bool Ask(YesterdayInfo yesterday, IRandomSource random)
    {
        return _rule.ShouldAsk(yesterday, random);
    }
}
