using System.Collections.Generic;
using SkipMaster.Domain;

namespace SkipMaster.Simulation;

// прогон до конца семестра или до вылета
public interface ISemesterRunner
{
    SemesterResult Run(IReadOnlyList<Teacher> teachers, ISkipStrategy strategy, IStudentHistory history);
}
