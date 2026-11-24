namespace SkipMaster.Domain.Rules;

/// <summary>
/// Спросить случайно с вероятностью 50 %.
/// </summary>
public sealed class RandomAskRule : IAskRule
{
    private const double AskProbability = 0.5;

    public bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random)
    {
        return random.NextDouble() < AskProbability;
    }
}
