namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a character's schedule.
/// </summary>
public class Schedule
{
  #region Properties

  /// <summary>
  /// The character linked to the schedule.
  /// </summary>
  public Guid CharacterId { get; }

  /// <summary>
  /// The schedule's entries.
  /// </summary>
  public List<ScheduleEntry> Entries { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Schedule(Guid characterId, List<ScheduleEntry> entries)
  {
    CharacterId = characterId;
    Entries = entries.OrderBy(e => e.StartHour).ToList();
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Schedule Create(Guid characterId, IEnumerable<ScheduleEntry> entries) =>
    new(characterId, entries.ToList());

  /// <summary>
  /// Gets the current entry depending on the current day and time.
  /// </summary>
  public ScheduleEntry? GetCurrentEntry(DayOfWeek currentDay, TimeSpan currentTime)
  {
    return Entries.LastOrDefault(e => e.Day == currentDay && e.StartHour <= currentTime);
  }

  #endregion
}

public record ScheduleEntry(DayOfWeek Day, TimeSpan StartHour, TimeSpan EndHour, Guid? LocationId, Guid? RoomId);
