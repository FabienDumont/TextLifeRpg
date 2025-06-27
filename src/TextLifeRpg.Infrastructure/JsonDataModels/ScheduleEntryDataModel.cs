namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// JSON data model representing a schedule's entry.
/// </summary>
public class ScheduleEntryDataModel
{
  public DayOfWeek Day { get; init; }
  public TimeSpan StartHour { get; init; }
  public TimeSpan EndHour { get; init; }
  public Guid? LocationId { get; init; }
  public Guid? RoomId { get; init; }
}
