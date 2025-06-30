using TextLifeRpg.Domain.Constants;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Job data seeder.
/// </summary>
public class JobSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var jobs = new List<JobDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        Name = JobNames.GarbageCollector,
        HourIncome = 12,
        MaxWorkers = 5
      },
      new()
      {
        Id = Guid.NewGuid(),
        Name = JobNames.Janitor,
        HourIncome = 13,
        MaxWorkers = 5
      },
      new()
      {
        Id = Guid.NewGuid(),
        Name = JobNames.DeliveryDriver,
        HourIncome = 14,
        MaxWorkers = 5
      },
      new()
      {
        Id = Guid.NewGuid(),
        Name = JobNames.CollegeTeacher,
        HourIncome = 30,
        MaxWorkers = 10
      }
    };

    foreach (var job in jobs)
    {
      await context.Jobs.AddAsync(job).ConfigureAwait(false);
    }

    await context.SaveChangesAsync().ConfigureAwait(false);
  }

  #endregion
}
