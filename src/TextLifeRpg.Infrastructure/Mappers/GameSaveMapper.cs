using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="GameSave" /> domain models and <see cref="GameSaveDataModel" /> JSON data
/// models.
/// </summary>
public static class GameSaveMapper
{
  #region Methods

  /// <summary>
  /// Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static GameSave ToDomain(this GameSaveDataModel dataModel)
  {
    var gameSave = dataModel.Map(i => GameSave.Load(
        i.Id, i.PlayerCharacterId, i.World.ToDomain(), i.TextLines.ToDomainCollection()
      )
    );

    if (dataModel.InteractingNpcId is not null && dataModel.NpcInteractionType == NpcInteractionType.Dialogue)
    {
      gameSave.StartDialogue((Guid)dataModel.InteractingNpcId);
    }

    return gameSave;
  }

  /// <summary>
  /// Maps a collection of JSON data models to domain models.
  /// </summary>
  public static List<GameSave> ToDomainCollection(this IEnumerable<GameSaveDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static GameSaveDataModel ToDataModel(this GameSave domain)
  {
    return domain.Map(d => new GameSaveDataModel
      {
        Id = d.Id,
        Name = d.Name,
        World = d.World.ToDataModel(),
        PlayerCharacterId = d.PlayerCharacterId,
        InteractingNpcId = d.InteractingNpcId,
        NpcInteractionType = d.NpcInteractionType,
        TextLines = d.TextLines.ToDataModelCollection()
      }
    );
  }

  #endregion
}
