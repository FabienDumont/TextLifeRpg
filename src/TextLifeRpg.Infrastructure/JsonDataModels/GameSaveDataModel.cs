namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// JSON data model representing a game save.
/// </summary>
public class GameSaveDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Name of the save file.
  /// </summary>
  public string Name { get; init; } = string.Empty;

  /// <summary>
  /// The world state at the time of the save.
  /// </summary>
  public required WorldDataModel World { get; init; }

  /// <summary>
  /// Identifier of the player character within the world.
  /// </summary>
  public Guid PlayerCharacterId { get; init; }

  /// <summary>
  /// Timestamp when the game was saved.
  /// </summary>
  public DateTime SavedAt { get; init; }

  /// <summary>
  /// List of text lines stored in the game save (with color formatting).
  /// </summary>
  public List<TextLineDataModel> TextLines { get; init; } = new();

  #endregion
}
