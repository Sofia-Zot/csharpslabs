using SkipMaster.Domain;
using SkipMaster.Domain.Rules;
using Xunit;

namespace SkipMaster.Tests;

public class RandomAskRuleTests
{
    [Theory]
    [InlineData(0.4, true)]
    [InlineData(0.6, false)]
    public void ShouldAsk_SubstitutedDraw_ReturnsExpected(double draw, bool expected)
    {
        var rule = new RandomAskRule();
        var random = FakeRandomSource.WithDoubles(draw);

        bool result = rule.ShouldAsk(YesterdayInfo.AllNotAsked(), random);

        Assert.Equal(expected, result);
    }
}
