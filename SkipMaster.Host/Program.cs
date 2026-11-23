using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkipMaster.Domain;
using SkipMaster.Host;
using SkipMaster.Simulation;
using SkipMaster.Strategies;

Console.OutputEncoding = Encoding.UTF8;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.Configure<SimulationOptions>(context.Configuration.GetSection("SimulationOptions"));

    services.AddSingleton<IRandomSource, DefaultRandomSource>();
    services.AddSingleton<IAskRuleGenerator, RuleGenerator>();
    services.AddSingleton<TeacherProvider>();
    services.AddSingleton<IStudentHistory, InMemoryStudentHistory>();
    services.AddSingleton<ISkipStrategy, AlwaysAttendStrategy>();
    services.AddSingleton<IDayGenerator, DayGenerator>();
    services.AddSingleton<ISemesterRunner, SemesterRunner>();

    services.AddHostedService<SemesterHostedService>();
});

builder.Build().Run();
