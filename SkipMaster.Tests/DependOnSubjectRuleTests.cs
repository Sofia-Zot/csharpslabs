using SkipMaster.Domain;
using SkipMaster.Domain.Rules;
using Xunit;

namespace SkipMaster.Tests;

public class DependOnSubjectRuleTests
{
    [Fact]
    public void ShouldAsk_YesterdayAskedByA_ReturnsTrue()
    {
        var rule = new DependOnSubjectRule(Subject.Physics);
        YesterdayInfo yesterday = TestYesterdays.OnlyAsked(Subject.Physics);

        bool result = rule.ShouldAsk(yesterday, FakeRandomSource.WithDoubles());

        Assert.True(result);
    }

    [Fact]
    public void ShouldAsk_YesterdayAskedByOtherSubject_ReturnsFalse()
    {
        var rule = new DependOnSubjectRule(Subject.Physics);
        YesterdayInfo yesterday = TestYesterdays.OnlyAsked(Subject.Calculus);

        bool result = rule.ShouldAsk(yesterday, FakeRandomSource.WithDoubles());

        Assert.False(result);
    }

    [Fact]
    public void ShouldAsk_FirstDay_ReturnsFalse()
    {
        var rule = new DependOnSubjectRule(Subject.Physics);

        bool result = rule.ShouldAsk(YesterdayInfo.AllNotAsked(), FakeRandomSource.WithDoubles());

        Assert.False(result);
    }
}
