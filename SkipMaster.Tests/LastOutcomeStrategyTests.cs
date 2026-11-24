using SkipMaster.Domain;
using SkipMaster.Simulation;
using SkipMaster.Strategies;
using Xunit;

namespace SkipMaster.Tests;

public class LastOutcomeStrategyTests
{
    private const int Today = 4;

    [Fact]
    public void DecideDay_EmptyHistory_AttendsAll()
    {
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, new InMemoryStudentHistory());

        Assert.All(plan, attend => Assert.True(attend));
    }

    [Fact]
    public void DecideDay_StablyNotAsked_SkipsSubject()
    {
        var history = new InMemoryStudentHistory();
        RecordAttended(history, Subject.Physics, asked: false, 1, 2, 3);
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        Assert.False(plan[(int)Subject.Physics]);
    }

    [Fact]
    public void DecideDay_StablyAsked_AttendsSubject()
    {
        var history = new InMemoryStudentHistory();
        RecordAttended(history, Subject.Physics, asked: true, 1, 2, 3);
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        Assert.True(plan[(int)Subject.Physics]);
    }

    [Fact]
    public void DecideDay_MixedHistoryLastNotAsked_SkipsSubject()
    {
        var history = new InMemoryStudentHistory();
        RecordAttended(history, Subject.Programming, asked: true, 1, 2);
        RecordAttended(history, Subject.Programming, asked: false, 3);
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        Assert.False(plan[(int)Subject.Programming]);
    }

    [Fact]
    public void DecideDay_MixedHistoryLastAsked_AttendsSubject()
    {
        var history = new InMemoryStudentHistory();
        RecordAttended(history, Subject.Programming, asked: false, 1, 2);
        RecordAttended(history, Subject.Programming, asked: true, 3);
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        Assert.True(plan[(int)Subject.Programming]);
    }

    [Fact]
    public void DecideDay_SubjectsDecideIndependently()
    {
        var history = new InMemoryStudentHistory();
        RecordAttended(history, Subject.Calculus, asked: false, 1, 2, 3);    // стабильно не спрашивали -> прогул
        RecordAttended(history, Subject.LinearAlgebra, asked: true, 1, 2, 3); // стабильно спрашивали -> идём
        RecordAttended(history, Subject.DiscreteMath, asked: false, 1);
        RecordAttended(history, Subject.DiscreteMath, asked: true, 2, 3);     // смешанная, последний раз спросили -> идём
        RecordAttended(history, Subject.Programming, asked: true, 1, 2);
        RecordAttended(history, Subject.Programming, asked: false, 3);        // смешанная, последний раз не спросили -> прогул
        RecordAttended(history, Subject.English, asked: true, 1);
        history.Record(2, Subject.English, attended: false, asked: false);    // прогулы не считаются -> идём
        history.Record(3, Subject.English, attended: false, asked: false);
        // Physics не посещался -> идём
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        Assert.False(plan[(int)Subject.Calculus]);
        Assert.True(plan[(int)Subject.LinearAlgebra]);
        Assert.True(plan[(int)Subject.DiscreteMath]);
        Assert.False(plan[(int)Subject.Programming]);
        Assert.True(plan[(int)Subject.Physics]);
        Assert.True(plan[(int)Subject.English]);
    }

    [Fact]
    public void DecideDay_UnattendedDaysDoNotCount()
    {
        var history = new InMemoryStudentHistory();
        RecordAttended(history, Subject.Physics, asked: true, 1);
        history.Record(2, Subject.Physics, attended: false, asked: false);
        history.Record(3, Subject.Physics, attended: false, asked: false);
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        // последний посещённый день - 1-й, там спрашивали -> идём
        Assert.True(plan[(int)Subject.Physics]);
    }

    private static void RecordAttended(IStudentHistory history, Subject subject, bool asked, params int[] days)
    {
        foreach (int day in days)
        {
            history.Record(day, subject, attended: true, asked: asked);
        }
    }
}
