using System;
using System.Collections.Generic;
using System.Linq;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

/// <summary>
/// Шесть преподавателей, каждому на семестр - случайное правило.
/// </summary>
public sealed class TeacherProvider(IAskRuleGenerator ruleGenerator)
{
    public IReadOnlyList<Teacher> Teachers { get; } = Enum.GetValues<Subject>()
        .Select(subject => new Teacher(subject, ruleGenerator.Generate()))
        .ToArray();
}
