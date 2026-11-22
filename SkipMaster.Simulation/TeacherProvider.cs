using System;
using System.Collections.Generic;
using System.Linq;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

// шесть преподавателей, каждому на семестр - случайное правило
public sealed class TeacherProvider
{
    public TeacherProvider(IAskRuleGenerator ruleGenerator)
    {
        Teachers = Enum.GetValues<Subject>()
            .Select(subject => new Teacher(subject, ruleGenerator.Generate()))
            .ToArray();
    }

    public IReadOnlyList<Teacher> Teachers { get; }
}
