using SkipMaster.Domain;
using SkipMaster.Domain.Rules;

namespace SkipMaster.Simulation;

public sealed class RuleGenerator : IAskRuleGenerator
{
    private const int RuleTypeCount = 3;

    private readonly IRandomSource _random;

    public RuleGenerator(IRandomSource random)
    {
        _random = random;
    }

    public IAskRule Generate(Subject ownSubject)
    {
        // 0, 1 или 2 -> какое правило досталось преподу
        int ruleType = _random.Next(RuleTypeCount);
        return ruleType switch
        {
            0 => new RandomAskRule(),
            1 => new DependOnSubjectRule(PickSubject()),
            _ => CreateXorRule(),
        };
    }

    private XorSubjectsRule CreateXorRule()
    {
        // Б берем со смещением по кругу, чтобы не совпал с А
        var subjectA = (Subject)_random.Next(SimulationDefaults.SubjectsPerDay);
        int offset = 1 + _random.Next(SimulationDefaults.SubjectsPerDay - 1);
        var subjectB = (Subject)(((int)subjectA + offset) % SimulationDefaults.SubjectsPerDay);
        return new XorSubjectsRule(subjectA, subjectB);
    }

    private Subject PickSubject()
    {
        return (Subject)_random.Next(SimulationDefaults.SubjectsPerDay);
    }
}
