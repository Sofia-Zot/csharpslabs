using SkipMaster.Domain;
using SkipMaster.Domain.Rules;

namespace SkipMaster.Simulation;

public sealed class RuleGenerator(IRandomSource random) : IAskRuleGenerator
{
    private const int RuleTypeCount = 3;

    public IAskRule Generate()
    {
        // 0, 1 или 2 -> какое правило досталось преподу
        int ruleType = random.Next(RuleTypeCount);
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
        var subjectA = (Subject)random.Next(SimulationDefaults.SubjectsPerDay);
        int offset = 1 + random.Next(SimulationDefaults.SubjectsPerDay - 1);
        var subjectB = (Subject)(((int)subjectA + offset) % SimulationDefaults.SubjectsPerDay);
        return new XorSubjectsRule(subjectA, subjectB);
    }

    private Subject PickSubject()
    {
        return (Subject)random.Next(SimulationDefaults.SubjectsPerDay);
    }
}
