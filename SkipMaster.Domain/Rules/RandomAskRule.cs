namespace SkipMaster.Domain.Rules;

public sealed class RandomAskRule : IAskRule
{
    private const double AskProbability = 0.5;

    // 50/50
    public bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random)
    {
        return random.NextDouble() < AskProbability;
    }
}
