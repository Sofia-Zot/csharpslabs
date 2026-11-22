using System;
using System.Linq;
using System.Text;
using SkipMaster.Domain;
using SkipMaster.Simulation;
using SkipMaster.Strategies;

// пока руками, di будет в лабе 2

Console.OutputEncoding = Encoding.UTF8;

IRandomSource random = new DefaultRandomSource();
var ruleGenerator = new RuleGenerator(random);

// каждому преподу на семестр - случайное правило
Teacher[] teachers = Enum.GetValues<Subject>()
    .Select(subject => new Teacher(subject, ruleGenerator.Generate(subject)))
    .ToArray();

var history = new InMemoryStudentHistory();
ISkipStrategy strategy = new AlwaysAttendStrategy();
var dayGenerator = new DayGenerator(random);
var semesterRunner = new SemesterRunner(dayGenerator);

SemesterResult result = semesterRunner.Run(teachers, strategy, history);

Console.WriteLine(result.Exploded ? $"Студент вылетел на дне {result.DaysCompleted}" : "Семестр завершен");
Console.WriteLine($"Общее удовольствие: {result.TotalPleasure}");
Console.WriteLine($"Среднее удовольствие в день: {result.AveragePerDay}");
