using SkipMaster.Domain;
using Xunit;

namespace SkipMaster.Tests;

public class YesterdayInfoTests
{
    [Fact]
    public void WasAsked_ShuffledOutcomes_ReturnsAnswersBySubject()
    {
        var outcomes = new[]
        {
            new SubjectOutcome(Subject.English, Attended: true, Asked: false),
            new SubjectOutcome(Subject.Calculus, Attended: true, Asked: true),
            new SubjectOutcome(Subject.Physics, Attended: true, Asked: true),
        };

        var yesterday = new YesterdayInfo(outcomes);

        Assert.True(yesterday.WasAsked(Subject.Calculus));
        Assert.True(yesterday.WasAsked(Subject.Physics));
        Assert.False(yesterday.WasAsked(Subject.English));
        Assert.False(yesterday.WasAsked(Subject.DiscreteMath)); // неполный список -> не спрашивали
    }
}
