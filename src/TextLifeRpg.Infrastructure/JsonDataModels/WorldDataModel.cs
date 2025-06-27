namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
///   JSON data model representing the game world at a point in time.
/// </summary>
public class WorldDataModel
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  ///   The current in-game date and time.
  /// </summary>
  public DateTime CurrentDate { get; init; }

  /// <summary>
  ///   List of characters present in the world.
  /// </summary>
  public List<CharacterDataModel> Characters { get; init; } = [];

  /// <summary>
  ///   List of relationships present in the world.
  /// </summary>
  public List<RelationshipDataModel> Relationships { get; init; } = [];

  /// <summary>
  ///   List of character schedules.
  /// </summary>
  public List<ScheduleDataModel> Schedules { get; init; } = [];

  #endregion
}
