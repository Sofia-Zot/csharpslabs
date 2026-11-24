using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SkipMaster.Domain;
using SkipMaster.Simulation;
using Xunit;

namespace SkipMaster.Tests;

public class DayGeneratorTests
{
    [Fact]
    public void RunDay_AttendedAndNotAsked_PairGivesNoPleasure()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var history = new InMemoryStudentHistory();

        var (outcome, _) = dayGenerator.RunDay(
            1, AllTeachers(ask: false), PlanStrategy(AllAttendPlan()), history, YesterdayInfo.AllNotAsked());

        Assert.False(outcome.Exploded);
        Assert.Equal(1, outcome.Pleasure);
    }

    [Fact]
    public void RunDay_AttendedAndAsked_PairGivesNoPleasure()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var history = new InMemoryStudentHistory();

        var (outcome, _) = dayGenerator.RunDay(
            1, AllTeachers(ask: true), PlanStrategy(AllAttendPlan()), history, YesterdayInfo.AllNotAsked());

        Assert.False(outcome.Exploded);
        Assert.Equal(1, outcome.Pleasure);
    }

    [Fact]
    public void RunDay_SkippedAndNotAsked_PairGivesOnePleasure()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var history = new InMemoryStudentHistory();
        var plan = new[] { true, true, true, false, true, true };

        var (outcome, _) = dayGenerator.RunDay(
            1, AllTeachers(ask: false), PlanStrategy(plan), history, YesterdayInfo.AllNotAsked());

        Assert.False(outcome.Exploded);
        Assert.Equal(2, outcome.Pleasure);
        Assert.False(outcome.Subjects[3].Attended);
    }

    [Fact]
    public void RunDay_SkippedAndAsked_DayExplodes()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var history = new InMemoryStudentHistory();
        var plan = new[] { true, true, true, false, true, true };

        var (outcome, _) = dayGenerator.RunDay(
            1, AllTeachers(ask: true), PlanStrategy(plan), history, YesterdayInfo.AllNotAsked());

        Assert.True(outcome.Exploded);
        Assert.Equal(2, outcome.Pleasure);
    }

    [Fact]
    public void RunDay_DayEnds_YesterdayMatchesTodaysAsks()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var history = new InMemoryStudentHistory();

        var (_, nextYesterday) = dayGenerator.RunDay(
            1,
            Teachers(true, false, false, false, false, false),
            PlanStrategy(AllAttendPlan()),
            history,
            YesterdayInfo.AllNotAsked());

        Assert.True(nextYesterday.WasAsked(Subject.Calculus));
        Assert.False(nextYesterday.WasAsked(Subject.LinearAlgebra));
        Assert.False(nextYesterday.WasAsked(Subject.English));
    }

    [Fact]
    public void RunDay_AnyDay_RecordsOutcomesToHistory()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var history = new InMemoryStudentHistory();
        var plan = new[] { true, true, true, false, true, true };

        dayGenerator.RunDay(
            1,
            Teachers(true, false, false, false, false, false),
            PlanStrategy(plan),
            history,
            YesterdayInfo.AllNotAsked());

        Assert.True(history.WasAsked(1, Subject.Calculus));
        Assert.False(history.WasAsked(1, Subject.English));
        Assert.False(history.Attended(1, Subject.Programming));
        Assert.True(history.Attended(1, Subject.English));
    }

    [Fact]
    public void RunDay_PlanLengthNotEqualTeachersCount_Throws()
    {
        var dayGenerator = new DayGenerator(FakeRandomSource.WithInts());
        var shortPlan = new[] { true, true, true, true, true };

        Assert.Throws<ArgumentException>(() => dayGenerator.RunDay(
            1,
            AllTeachers(ask: false),
            PlanStrategy(shortPlan),
            new InMemoryStudentHistory(),
            YesterdayInfo.AllNotAsked()));
    }

    private static bool[] AllAttendPlan()
    {
        return new[] { true, true, true, true, true, true };
    }

    private static ISkipStrategy PlanStrategy(bool[] plan)
    {
        var strategy = new Mock<ISkipStrategy>();
        strategy.Setup(s => s.DecideDay(It.IsAny<int>(), It.IsAny<IReadOnlyStudentHistory>()))
            .Returns(plan);
        return strategy.Object;
    }

    private static IReadOnlyList<Teacher> AllTeachers(bool ask)
    {
        return Teachers(Enumerable.Repeat(ask, SimulationDefaults.SubjectsPerDay).ToArray());
    }

    private static IReadOnlyList<Teacher> Teachers(params bool[] asks)
    {
        return Enum.GetValues<Subject>()
            .Select((subject, i) =>
            {
                var rule = new Mock<IAskRule>();
                rule.Setup(r => r.ShouldAsk(It.IsAny<YesterdayInfo>(), It.IsAny<IRandomSource>()))
                    .Returns(asks[i]);
                return new Teacher(subject, rule.Object);
            })
            .ToArray();
    }
}
