using System;
using SkipMaster.Domain;
using SkipMaster.Domain.Rules;
using Xunit;

namespace SkipMaster.Tests;

public class XorSubjectsRuleTests
{
    [Fact]
    public void ShouldAsk_OnlyAAsked_ReturnsTrue()
    {
        var rule = new XorSubjectsRule(Subject.Calculus, Subject.English);

        bool result = rule.ShouldAsk(TestYesterdays.OnlyAsked(Subject.Calculus), FakeRandomSource.WithDoubles());

        Assert.True(result);
    }

    [Fact]
    public void ShouldAsk_OnlyBAsked_ReturnsTrue()
    {
        var rule = new XorSubjectsRule(Subject.Calculus, Subject.English);

        bool result = rule.ShouldAsk(TestYesterdays.OnlyAsked(Subject.English), FakeRandomSource.WithDoubles());

        Assert.True(result);
    }

    [Fact]
    public void ShouldAsk_BothAsked_ReturnsFalse()
    {
        var rule = new XorSubjectsRule(Subject.Calculus, Subject.English);

        bool result = rule.ShouldAsk(
            TestYesterdays.OnlyAsked(Subject.Calculus, Subject.English),
            FakeRandomSource.WithDoubles());

        Assert.False(result);
    }

    [Fact]
    public void ShouldAsk_NeitherAsked_ReturnsFalse()
    {
        var rule = new XorSubjectsRule(Subject.Calculus, Subject.English);

        bool result = rule.ShouldAsk(YesterdayInfo.AllNotAsked(), FakeRandomSource.WithDoubles());

        Assert.False(result);
    }

    [Fact]
    public void Constructor_SameSubjects_Throws()
    {
        Assert.Throws<ArgumentException>(() => new XorSubjectsRule(Subject.Calculus, Subject.Calculus));
    }
}
