using System;

namespace SkipMaster.Domain;

public readonly record struct YesterdayInfo(
    bool AskedCalculus,
    bool AskedLinearAlgebra,
    bool AskedDiscreteMath,
    bool AskedProgramming,
    bool AskedPhysics,
    bool AskedEnglish)
{
    // начало семестра -> вчера нет
    public static YesterdayInfo AllNotAsked()
    {
        return new YesterdayInfo(false, false, false, false, false, false);
    }

    public bool WasAsked(Subject subject)
    {
        return subject switch
        {
            Subject.Calculus => AskedCalculus,
            Subject.LinearAlgebra => AskedLinearAlgebra,
            Subject.DiscreteMath => AskedDiscreteMath,
            Subject.Programming => AskedProgramming,
            Subject.Physics => AskedPhysics,
            Subject.English => AskedEnglish,
            _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, "Неизвестный предмет."),
        };
    }
}
