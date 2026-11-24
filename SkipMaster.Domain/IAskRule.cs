namespace SkipMaster.Domain;

/// <summary>
/// Три правила = три класса.
/// </summary>
public interface IAskRule
{
    bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random);
}
