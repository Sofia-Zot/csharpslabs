namespace SkipMaster.Domain;

// весь рандом приложения - только через это (тестировать будет проще)
public interface IRandomSource
{
    double NextDouble();

    int Next(int maxExclusive);
}
