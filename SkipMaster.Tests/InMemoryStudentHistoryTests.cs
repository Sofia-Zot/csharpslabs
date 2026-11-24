using SkipMaster.Domain;
using SkipMaster.Simulation;
using Xunit;

namespace SkipMaster.Tests;

public class InMemoryStudentHistoryTests
{
    [Fact]
    public void WasAsked_UnvisitedDay_ReturnsNull()
    {
        var history = new InMemoryStudentHistory();

        Assert.Null(history.WasAsked(5, Subject.Calculus));
    }

    [Fact]
    public void Attended_UnvisitedDay_ReturnsFalse()
    {
        var history = new InMemoryStudentHistory();

        Assert.False(history.Attended(5, Subject.English));
    }

    [Fact]
    public void Record_AfterVisit_ReturnsRecordedValues()
    {
        var history = new InMemoryStudentHistory();

        history.Record(3, Subject.Programming, attended: true, asked: false);

        Assert.True(history.Attended(3, Subject.Programming));
        Assert.False(history.WasAsked(3, Subject.Programming));
    }

    [Fact]
    public void Record_SameDayAndSubjectTwice_KeepsLastRecord()
    {
        var history = new InMemoryStudentHistory();

        history.Record(3, Subject.Programming, attended: true, asked: true);
        history.Record(3, Subject.Programming, attended: false, asked: true);

        Assert.False(history.Attended(3, Subject.Programming));
        Assert.Null(history.WasAsked(3, Subject.Programming)); // исход прогуленной пары не виден
    }

    [Fact]
    public void Record_DifferentDays_DoNotOverlap()
    {
        var history = new InMemoryStudentHistory();

        history.Record(2, Subject.Physics, attended: true, asked: true);
        history.Record(3, Subject.Physics, attended: true, asked: false);

        Assert.True(history.WasAsked(2, Subject.Physics));
        Assert.False(history.WasAsked(3, Subject.Physics));
        Assert.True(history.Attended(2, Subject.Physics));
        Assert.True(history.Attended(3, Subject.Physics));
    }

    [Fact]
    public void WasAsked_SkippedPair_ReturnsNull()
    {
        var history = new InMemoryStudentHistory();

        history.Record(3, Subject.Physics, attended: false, asked: true);

        Assert.Null(history.WasAsked(3, Subject.Physics));
        Assert.False(history.Attended(3, Subject.Physics));
    }
}
