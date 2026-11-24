namespace SkipMaster.Domain;

/// <summary>
/// На старте семестра каждый препод получает свое правило.
/// </summary>
public interface IAskRuleGenerator
{
    IAskRule Generate();
}
