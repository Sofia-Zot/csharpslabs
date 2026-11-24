using System;
using System.Linq;
using SkipMaster.Domain;

namespace SkipMaster.Tests;

// фабрика «вчера» для тестов правил - отметили спросивших, остальное не спрашивали
internal static class TestYesterdays
{
    public static YesterdayInfo OnlyAsked(params Subject[] askedSubjects)
    {
        var outcomes = Enum.GetValues<Subject>()
            .Select(subject => new SubjectOutcome(subject, Attended: true, Asked: askedSubjects.Contains(subject)))
            .ToArray();

        return new YesterdayInfo(outcomes);
    }
}
