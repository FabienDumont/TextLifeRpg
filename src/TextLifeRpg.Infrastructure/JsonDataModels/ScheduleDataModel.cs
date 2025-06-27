namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
///   JSON data model representing a character's schedule.
/// </summary>
public class ScheduleDataModel
{
  public Guid CharacterId { get; init; }
  public List<ScheduleEntryDataModel> Entries { get; init; } = [];
}
