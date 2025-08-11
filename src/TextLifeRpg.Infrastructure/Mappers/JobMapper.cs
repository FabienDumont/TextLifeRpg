using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="Job" /> domain models and <see cref="JobDataModel" /> JSON data models.
/// </summary>
public static class JobMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Job ToDomain(this JobDataModel dataModel)
  {
    return dataModel.Map(i => Job.Load(i.Id, i.Name, i.HourIncome, i.MaxWorkers));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Job> ToDomainCollection(this IEnumerable<JobDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static JobDataModel ToDataModel(this Job domain)
  {
    return domain.Map(u => new JobDataModel
      {
        Id = u.Id,
        Name = u.Name,
        HourIncome = u.HourIncome,
        MaxWorkers = u.MaxWorkers
      }
    );
  }

  #endregion
}
