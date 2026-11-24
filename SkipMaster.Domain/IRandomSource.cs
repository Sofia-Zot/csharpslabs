namespace SkipMaster.Domain;

/// <summary>
/// Весь рандом приложения - только через это (тестировать будет проще).
/// </summary>
public interface IRandomSource
{
    double NextDouble();

    int Next(int maxExclusive);
}
