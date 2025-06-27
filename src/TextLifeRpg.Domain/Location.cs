namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing a location in the game world.
/// </summary>
public class Location
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  ///   Name of the location.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  ///   Flag is the location always open.
  /// </summary>
  public bool IsAlwaysOpen => OpeningHours.Count == 0;

  public List<LocationOpeningHours> OpeningHours { get; private init; } = [];

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor used internally.
  /// </summary>
  private Location(Guid id, string name)
  {
    Id = id;
    Name = name;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static Location Create(string name)
  {
    return new Location(Guid.NewGuid(), name);
  }

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static Location Load(Guid id, string name, List<LocationOpeningHours> openingHours)
  {
    return new Location(id, name)
    {
      OpeningHours = openingHours
    };
  }

  public bool IsOpenAt(DayOfWeek day, TimeSpan time)
  {
    if (IsAlwaysOpen)
    {
      return true;
    }

    foreach (var hours in OpeningHours)
    {
      if (hours.DayOfWeek != day)
      {
        if (hours.OpensAt > hours.ClosesAt)
        {
          var prevDay = day == DayOfWeek.Sunday ? DayOfWeek.Saturday : (DayOfWeek) ((int) day - 1);
          if (hours.DayOfWeek == prevDay && time <= hours.ClosesAt) return true;
        }

        continue;
      }

      if (hours.OpensAt <= hours.ClosesAt)
      {
        if (time >= hours.OpensAt && time <= hours.ClosesAt) return true;
      }
      else
      {
        // Cross-midnight window (e.g., 22:00 – 02:00 same day)
        if (time >= hours.OpensAt || time <= hours.ClosesAt) return true;
      }
    }

    return false;
  }

  /// <summary>
  ///   Gets the timeslots where a location is opened for a given day and given allowed durations.
  /// </summary>
  public List<(TimeSpan Start, TimeSpan End)> GetTimeSlots(
    DayOfWeek day, IReadOnlyCollection<TimeSpan> allowedDurations
  )
  {
    var slots = new List<(TimeSpan Start, TimeSpan End)>();

    List<(TimeSpan OpensAt, TimeSpan ClosesAt)> openingRanges;

    openingRanges = IsAlwaysOpen
      ? [(TimeSpan.Zero, TimeSpan.FromHours(24))]
      : OpeningHours.Where(o => o.DayOfWeek == day).Select(o => (o.OpensAt, o.ClosesAt)).ToList();

    foreach (var (opensAt, closesAt) in openingRanges)
    {
      var current = new TimeSpan(opensAt.Hours, 0, 0);
      if (current < opensAt)
      {
        current = current.Add(TimeSpan.FromHours(1));
      }

      while (current < closesAt)
      {
        foreach (var duration in allowedDurations)
        {
          var end = current + duration;
          if (end <= closesAt)
          {
            slots.Add((current, end));
          }
        }

        current = current.Add(TimeSpan.FromHours(1));
      }
    }

    return slots;
  }

  #endregion
}
