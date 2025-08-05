using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="DialogueOption" /> domain models and <see cref="DialogueOptionDataModel" /> EF data
/// models.
/// </summary>
public static class DialogueOptionMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static DialogueOption ToDomain(this DialogueOptionDataModel dataModel)
  {
    return dataModel.Map(i => DialogueOption.Load(i.Id, i.Label));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<DialogueOption> ToDomainCollection(this IEnumerable<DialogueOptionDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static DialogueOptionDataModel ToDataModel(this DialogueOption domain)
  {
    return domain.Map(d => new DialogueOptionDataModel
      {
        Id = d.Id,
        Label = d.Label
      }
    );
  }

  #endregion
}
