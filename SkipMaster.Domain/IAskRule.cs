namespace SkipMaster.Domain;

// три правила = три класса
public interface IAskRule
{
    bool ShouldAsk(YesterdayInfo yesterday, IRandomSource random);
}
