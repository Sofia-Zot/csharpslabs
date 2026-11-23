using SkipMaster.Domain;
using SkipMaster.Domain.Rules;
using SkipMaster.Simulation;
using Xunit;

namespace SkipMaster.Tests;

public class RuleGeneratorTests
{
    [Fact]
    public void Generate_RuleTypeZero_ReturnsRandomAskRule()
    {
        var generator = new RuleGenerator(FakeRandomSource.WithInts(0));

        IAskRule rule = generator.Generate();

        Assert.IsType<RandomAskRule>(rule);
    }

    [Fact]
    public void Generate_RuleTypeOne_ReturnsDependRuleWithPickedSubject()
    {
        var generator = new RuleGenerator(FakeRandomSource.WithInts(1, 4));

        var rule = Assert.IsType<DependOnSubjectRule>(generator.Generate());

        Assert.Equal(Subject.Physics, rule.SubjectA);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(2, 3)]
    [InlineData(5, 4)]
    [InlineData(1, 2)]
    public void Generate_RuleTypeTwo_SubjectsAlwaysDiffer(int subjectA, int offset)
    {
        var generator = new RuleGenerator(FakeRandomSource.WithInts(2, subjectA, offset));

        var rule = Assert.IsType<XorSubjectsRule>(generator.Generate());

        int expectedB = (subjectA + 1 + offset) % SimulationDefaults.SubjectsPerDay;
        Assert.Equal((Subject)subjectA, rule.SubjectA);
        Assert.Equal((Subject)expectedB, rule.SubjectB);
        Assert.NotEqual(rule.SubjectA, rule.SubjectB);
    }
}
