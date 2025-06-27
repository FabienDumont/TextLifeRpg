namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a location's opening hours in the game world.
/// </summary>
public class LocationOpeningHours
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// The location's identifier.
  /// </summary>
  public Guid LocationId { get; }

  /// <summary>
  /// The day of the week this opening period applies to.
  /// </summary>
  public DayOfWeek DayOfWeek { get; }

  /// <summary>
  /// Time when the location opens.
  /// </summary>
  public TimeSpan OpensAt { get; }

  /// <summary>
  /// Time when the location closes.
  /// </summary>
  public TimeSpan ClosesAt { get; }

  #endregion

  #region Ctors

  private LocationOpeningHours(Guid id, Guid locationId, DayOfWeek dayOfWeek, TimeSpan opensAt, TimeSpan closesAt)
  {
    Id = id;
    LocationId = locationId;
    DayOfWeek = dayOfWeek;
    OpensAt = opensAt;
    ClosesAt = closesAt;
  }

  #endregion

  #region Methods

  public static LocationOpeningHours Create(Guid locationId, DayOfWeek dayOfWeek, TimeSpan opensAt, TimeSpan closesAt)
  {
    return new LocationOpeningHours(Guid.NewGuid(), locationId, dayOfWeek, opensAt, closesAt);
  }

  public static LocationOpeningHours Load(
    Guid id, Guid locationId, DayOfWeek dayOfWeek, TimeSpan opensAt, TimeSpan closesAt
  )
  {
    return new LocationOpeningHours(id, locationId, dayOfWeek, opensAt, closesAt);
  }

  #endregion
}
