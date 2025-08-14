using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="DialogueOptionResult" /> domain models and
/// <see cref="DialogueOptionResultDataModel" /> EF data models.
/// </summary>
public static class DialogueOptionResultMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static DialogueOptionResult ToDomain(this DialogueOptionResultDataModel dataModel)
  {
    return dataModel.Map(i => DialogueOptionResult.Load(
        i.Id, i.DialogueOptionId, i.TargetRelationshipValueChange, i.ActorLearnFact, i.ActorTargetSpecialAction, i.EndDialogue
      )
    );
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<DialogueOptionResult> ToDomainCollection(
    this IEnumerable<DialogueOptionResultDataModel> dataModels
  )
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static DialogueOptionResultDataModel ToDataModel(this DialogueOptionResult domain)
  {
    return domain.Map(d => new DialogueOptionResultDataModel
      {
        Id = d.Id,
        DialogueOptionId = d.DialogueOptionId,
        TargetRelationshipValueChange = d.TargetRelationshipValueChange,
        ActorLearnFact = d.ActorLearnFact,
        ActorTargetSpecialAction = d.ActorTargetSpecialAction,
        EndDialogue = d.EndDialogue
      }
    );
  }

  #endregion
}
