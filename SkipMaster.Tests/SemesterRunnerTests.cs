using System.Linq;
using Moq;
using SkipMaster.Domain;
using SkipMaster.Simulation;
using Xunit;

namespace SkipMaster.Tests;

public class SemesterRunnerTests
{
    [Fact]
    public void MakeStep_RegularDay_TotalPleasureGrowsByDayPleasure()
    {
        var runner = CreateRunner(attendAll: true, teachersAsk: false);

        DayOutcome? first = runner.MakeStep();

        Assert.NotNull(first);
        Assert.Equal(first.Pleasure, runner.TotalPleasure);
        Assert.Equal(2, runner.CurrentDay);

        runner.MakeStep();

        Assert.Equal(first.Pleasure * 2, runner.TotalPleasure);
    }

    [Fact]
    public void MakeStep_Explosion_ZeroesPleasureAndStops()
    {
        var runner = CreateRunner(attendAll: false, teachersAsk: true);

        DayOutcome? outcome = runner.MakeStep();

        Assert.NotNull(outcome);
        Assert.True(outcome.Exploded);
        Assert.True(runner.Exploded);
        Assert.Equal(0, runner.TotalPleasure);

        int dayBefore = runner.CurrentDay;

        Assert.Null(runner.MakeStep());
        Assert.Equal(dayBefore, runner.CurrentDay);
        Assert.Equal(0, runner.TotalPleasure);
        Assert.True(runner.Exploded);
    }

    [Fact]
    public void AveragePerDay_BeforeAnyDay_IsZero()
    {
        var runner = CreateRunner(attendAll: true, teachersAsk: false);

        Assert.Equal(0.0, runner.AveragePerDay);
    }

    [Fact]
    public void AveragePerDay_AfterFewDays_EqualsTotalOverDays()
    {
        var runner = CreateRunner(attendAll: false, teachersAsk: false);

        runner.MakeStep();
        runner.MakeStep();
        runner.MakeStep();

        Assert.Equal(21, runner.TotalPleasure);
        Assert.Equal(7.0, runner.AveragePerDay);
    }

    private static SemesterRunner CreateRunner(bool attendAll, bool teachersAsk)
    {
        var rule = new Mock<IAskRule>();
        rule.Setup(r => r.ShouldAsk(It.IsAny<YesterdayInfo>(), It.IsAny<IRandomSource>()))
            .Returns(teachersAsk);

        var ruleGenerator = new Mock<IAskRuleGenerator>();
        ruleGenerator.Setup(g => g.Generate()).Returns(rule.Object);

        var plan = Enumerable.Repeat(attendAll, SimulationDefaults.SubjectsPerDay).ToArray();
        var strategy = new Mock<ISkipStrategy>();
        strategy.Setup(s => s.DecideDay(It.IsAny<int>(), It.IsAny<IReadOnlyStudentHistory>()))
            .Returns(plan);

        return new SemesterRunner(
            new DayGenerator(FakeRandomSource.WithInts()),
            new TeacherProvider(ruleGenerator.Object),
            strategy.Object,
            new InMemoryStudentHistory());
    }
}
