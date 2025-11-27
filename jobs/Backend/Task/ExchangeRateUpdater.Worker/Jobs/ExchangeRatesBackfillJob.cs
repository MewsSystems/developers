namespace ExchangeRateUpdater.Worker.Jobs;

using System.Collections.Concurrent;
using Domain.Enums;
using System.Diagnostics;
using Application.Common.Utils;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Quartz;
using Services;

[DisallowConcurrentExecution]
public class ExchangeRatesBackfillJob : IJob
{
    private const int DefaultYears = 4;
    private const int DefaultParallelism = 8;
    private const string FlagFilePath = "/shared/worker_ready";

    private readonly ICzYearProcessor _czYearProcessor;
    private readonly IPerDayProcessor _perDayProcessor;
    private readonly ICacheRepository _cacheRepository;
    private readonly ILogger<ExchangeRatesBackfillJob> _logger;

    public ExchangeRatesBackfillJob(ICzYearProcessor czYearProcessor,
        IPerDayProcessor perDayProcessor,
        ICacheRepository cacheRepository,
        ILogger<ExchangeRatesBackfillJob> logger)
    {
        Ensure.Argument.NotNull(czYearProcessor, nameof(czYearProcessor));
        Ensure.Argument.NotNull(perDayProcessor, nameof(perDayProcessor));
        Ensure.Argument.NotNull(cacheRepository, nameof(cacheRepository));
        Ensure.Argument.NotNull(logger, nameof(logger));
        _czYearProcessor = czYearProcessor;
        _perDayProcessor = perDayProcessor;
        _cacheRepository = cacheRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        EnsureFlagFileDoesntExist();
        var overallSw = Stopwatch.StartNew();
        try
        {
            _logger.LogInformation(
                "Starting ExchangeRatesBackfillJob for last {Years} years with parallelism {Parallelism} at {Time}",
                DefaultYears, DefaultParallelism, DateTimeOffset.UtcNow);

            var end = DateTime.UtcNow.Date;
            var defaultStart = end.AddYears(-DefaultYears);

            var needBackfill = !(await CacheHasStartEndAsync(Language.CZ, defaultStart, end) &&
                                 await CacheHasStartEndAsync(Language.EN, defaultStart, end));

            if (needBackfill)
            {
                var czEarliest = await BackfillCzAsync(context, DefaultYears, DefaultParallelism);
                await BackfillEnAsync(context, DefaultYears, DefaultParallelism, czEarliest);
            }
            else
            {
                _logger.LogInformation("Cache already has data for the required date range. Skipping backfill.");
            }

            overallSw.Stop();
            _logger.LogInformation("ExchangeRatesBackfillJob completed at {Time} - total duration {Elapsed:c}",
                DateTimeOffset.UtcNow, overallSw.Elapsed);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("ExchangeRatesBackfillJob cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ExchangeRatesBackfillJob failed");
            throw;
        }
        finally
        {
            await NotifyBackfillCompleted();
        }
    }

    private void EnsureFlagFileDoesntExist()
    {
        if (File.Exists(FlagFilePath))
        {
            File.Delete(FlagFilePath);
        }
    }
    
    private async Task NotifyBackfillCompleted()
    {
        await File.WriteAllTextAsync(FlagFilePath, "ready");
    }
    
    private async Task<DateTime?> BackfillCzAsync(IJobExecutionContext context, int years, int parallelism)
    {
        var czSw = Stopwatch.StartNew();
        var czEarliest = await BackfillCzAsync(years, parallelism, context.CancellationToken);
        czSw.Stop();
        _logger.LogInformation("CZ backfill completed in {Elapsed:c}", czSw.Elapsed);
        return czEarliest;
    }

    private async Task<DateTime?> BackfillCzAsync(int years, int degreeOfParallelism,
        CancellationToken cancellationToken)
    {
        var end = DateTime.UtcNow.Date;
        var defaultStart = end.AddYears(-years);
        var yearsRange = Enumerable.Range(defaultStart.Year, end.Year - defaultStart.Year + 1).ToList();
        var foundDates = new ConcurrentBag<DateTime>();

        var options = new ParallelOptions
            { MaxDegreeOfParallelism = degreeOfParallelism, CancellationToken = cancellationToken };
        var processed = 0;
        await Parallel.ForEachAsync(yearsRange, options, async (year, ct) =>
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                var minForYear = await _czYearProcessor.ProcessYearAsync(year, ct);
                if (minForYear.HasValue) foundDates.Add(minForYear.Value);
            }
            finally
            {
                var count = Interlocked.Increment(ref processed);
                _logger.LogInformation("CZ backfill: processed {Processed}/{Total} years", count, yearsRange.Count);
            }
        });

        if (foundDates.IsEmpty) return null;
        var overallMin = foundDates.Min();
        _logger.LogInformation("CZ earliest date found: {Date}", overallMin);
        return overallMin;
    }

    private async Task BackfillEnAsync(IJobExecutionContext context, int years, int parallelism, DateTime? czEarliest)
    {
        var enSw = Stopwatch.StartNew();
        await BackfillPerDayAsync(years, parallelism, context.CancellationToken, czEarliest);
        enSw.Stop();
        _logger.LogInformation("EN backfill completed in {Elapsed:c}", enSw.Elapsed);
    }

    private async Task BackfillPerDayAsync(int years, int degreeOfParallelism, CancellationToken cancellationToken,
        DateTime? overrideStart = null)
    {
        var language = Language.EN;
        var end = DateTime.UtcNow.Date;
        var defaultStart = end.AddYears(-years);
        var start = overrideStart ?? defaultStart;

        _logger.LogInformation("Backfilling language {Language} from {Start} to {End}", language, start, end);

        var totalDays = (end - start).Days + 1;
        if (totalDays <= 0) return;

        var dates = Enumerable.Range(0, totalDays).Select(d => start.AddDays(d)).ToList();
        var processedDates = 0;
        var optionsDates = new ParallelOptions
            { MaxDegreeOfParallelism = degreeOfParallelism, CancellationToken = cancellationToken };

        await Parallel.ForEachAsync(dates, optionsDates, async (date, ct) =>
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                await _perDayProcessor.ProcessDateAsync(date, language, ct);
            }
            finally
            {
                var current = Interlocked.Increment(ref processedDates);
                if (current % 100 == 0 || current == dates.Count)
                    _logger.LogInformation("Backfill {Language}: processed {Processed}/{Total}", language, current,
                        dates.Count);
            }
        });

        _logger.LogInformation("Finished backfill for language {Language}", language);
    }

    private async Task<bool> CacheHasStartEndAsync(Language lang, DateTime start, DateTime end)
    {
        var startKey = CacheKeyHelper.RatesKey(lang, start);
        var endKey = CacheKeyHelper.RatesKey(lang, end);

        var startCached = await _cacheRepository.GetRatesDictionaryAsync(startKey);
        var endCached = await _cacheRepository.GetRatesDictionaryAsync(endKey);

        return startCached is not null && startCached.Any() && endCached is not null && endCached.Any();
    }
}