using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkipMaster.Simulation;

namespace SkipMaster.Host;

// фоновый цикл: день -> лог -> пауза; стоп по сtrl+с или при вылете
public sealed class SemesterHostedService : BackgroundService
{
    private readonly ISemesterRunner _semesterRunner;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly int _dayDelayMilliseconds;
    private readonly ILogger<SemesterHostedService> _logger;

    public SemesterHostedService(
        ISemesterRunner semesterRunner,
        IHostApplicationLifetime lifetime,
        IOptions<SimulationOptions> options,
        ILogger<SemesterHostedService> logger)
    {
        _semesterRunner = semesterRunner;
        _lifetime = lifetime;
        _dayDelayMilliseconds = options.Value.DayDelayMilliseconds;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested
               && !_semesterRunner.Exploded
               && !_semesterRunner.SemesterFinished)
        {
            int day = _semesterRunner.CurrentDay;
            _semesterRunner.MakeStep();

            if (_semesterRunner.Exploded)
            {
                _logger.LogWarning("Студент вылетел на дне {Day}, удовольствие обнулено", day);
                _lifetime.StopApplication();
                break;
            }

            if (_semesterRunner.SemesterFinished)
            {
                _logger.LogInformation(
                    "Семестр завершен: {Days} дней, общее удовольствие {Total}, среднее {Average:F2}",
                    day,
                    _semesterRunner.TotalPleasure,
                    _semesterRunner.AveragePerDay);
                _lifetime.StopApplication();
                break;
            }

            _logger.LogInformation(
                "День {Day}: общее удовольствие {Total}, среднее {Average:F2}",
                day,
                _semesterRunner.TotalPleasure,
                _semesterRunner.AveragePerDay);

            await Task.Delay(_dayDelayMilliseconds, stoppingToken);
        }
    }
}
