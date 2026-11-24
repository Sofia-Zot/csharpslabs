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
        RecordAttended(history, Subject.Calculus, asked: false, 1, 2, 3);
        RecordAttended(history, Subject.English, asked: true, 1, 2, 3);
        var strategy = new LastOutcomeStrategy();

        bool[] plan = strategy.DecideDay(Today, history);

        Assert.False(plan[(int)Subject.Calculus]);
        Assert.True(plan[(int)Subject.English]);
        Assert.True(plan[(int)Subject.DiscreteMath]); // не посещали -> идём
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
