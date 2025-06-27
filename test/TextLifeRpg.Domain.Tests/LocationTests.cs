using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class LocationTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Act
    var location = Location.Create(string.Empty);

    // Assert
    Assert.NotNull(location);
    Assert.NotEqual(Guid.Empty, location.Id);
    Assert.Equal(string.Empty, location.Name);
    Assert.True(location.IsAlwaysOpen);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Home";

    // Act
    var location = Location.Load(id, name, []);

    // Assert
    Assert.NotNull(location);
    Assert.Equal(id, location.Id);
    Assert.Equal(name, location.Name);
    Assert.True(location.IsAlwaysOpen);
  }

  [Theory]
  [InlineData(DayOfWeek.Monday, "09:00", true)]
  [InlineData(DayOfWeek.Monday, "07:59", false)]
  [InlineData(DayOfWeek.Monday, "18:00", true)]
  [InlineData(DayOfWeek.Monday, "18:01", false)]
  public void IsOpenAt_ShouldRespectStandardOpeningHours(DayOfWeek day, string timeString, bool expected)
  {
    // Arrange
    var openingHours = new List<LocationOpeningHours>
    {
      LocationOpeningHours.Create(Guid.NewGuid(), DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0))
    };

    var location = Location.Load(Guid.NewGuid(), "Test", openingHours);
    var time = TimeSpan.Parse(timeString);

    // Act
    var result = location.IsOpenAt(day, time);

    // Assert
    Assert.Equal(expected, result);
  }

  [Theory]
  [InlineData(DayOfWeek.Monday, "23:00", true)]
  [InlineData(DayOfWeek.Tuesday, "01:00", true)]
  [InlineData(DayOfWeek.Tuesday, "03:00", false)]
  [InlineData(DayOfWeek.Monday, "21:00", false)]
  public void IsOpenAt_ShouldSupportCrossMidnight(DayOfWeek day, string timeString, bool expected)
  {
    // Arrange
    var openingHours = new List<LocationOpeningHours>
    {
      // Open Monday 22:00 → 02:00 (Tuesday)
      LocationOpeningHours.Create(Guid.NewGuid(), DayOfWeek.Monday, new TimeSpan(22, 0, 0), new TimeSpan(2, 0, 0))
    };

    var location = Location.Load(Guid.NewGuid(), "Club", openingHours);
    var time = TimeSpan.Parse(timeString);

    // Act
    var result = location.IsOpenAt(day, time);

    // Assert
    Assert.Equal(expected, result);
  }

  [Fact]
  public void IsOpenAt_ShouldReturnTrue_WhenAlwaysOpen()
  {
    // Arrange
    var location = Location.Load(Guid.NewGuid(), "Park", []);

    // Act
    var result = location.IsOpenAt(DayOfWeek.Wednesday, new TimeSpan(3, 0, 0));

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void IsOpenAt_ShouldReturnFalse_WhenNoMatchingHours()
  {
    // Arrange
    var openingHours = new List<LocationOpeningHours>
    {
      LocationOpeningHours.Create(Guid.NewGuid(), DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(10, 0, 0))
    };

    var location = Location.Load(Guid.NewGuid(), "Office", openingHours);

    // Act
    var result = location.IsOpenAt(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0));

    // Assert
    Assert.False(result);
  }

  [Fact]
  public void GetTimeSlots_ShouldReturnSlots_WhenLocationIsAlwaysOpen()
  {
    // Arrange
    var location = Location.Load(Guid.NewGuid(), "Diner", []);
    var allowedDurations = new[] {TimeSpan.FromHours(1), TimeSpan.FromHours(2)};

    // Act
    var result = location.GetTimeSlots(DayOfWeek.Monday, allowedDurations);

    // Assert
    Assert.NotEmpty(result);
    Assert.Contains((TimeSpan.Zero, TimeSpan.FromHours(1)), result);
    Assert.Contains((TimeSpan.Zero, TimeSpan.FromHours(2)), result);
  }

  [Fact]
  public void GetTimeSlots_ShouldReturnCorrectSlots_ForDefinedHours()
  {
    // Arrange
    var hours = new List<LocationOpeningHours>
    {
      LocationOpeningHours.Create(Guid.NewGuid(), DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0))
    };
    var location = Location.Load(Guid.NewGuid(), "Shop", hours);
    var allowedDurations = new[] {TimeSpan.FromHours(1)};

    // Act
    var result = location.GetTimeSlots(DayOfWeek.Monday, allowedDurations);

    // Assert
    var expected = new List<(TimeSpan, TimeSpan)>
    {
      (new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)),
      (new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0)),
      (new TimeSpan(11, 0, 0), new TimeSpan(12, 0, 0))
    };

    Assert.Equal(expected, result);
  }

  [Fact]
  public void GetTimeSlots_ShouldAlignToFullHour_WhenStartIsNotOnHour()
  {
    // Arrange
    var hours = new List<LocationOpeningHours>
    {
      LocationOpeningHours.Create(Guid.NewGuid(), DayOfWeek.Tuesday, new TimeSpan(8, 30, 0), new TimeSpan(10, 30, 0))
    };
    var location = Location.Load(Guid.NewGuid(), "Café", hours);
    var allowedDurations = new[] {TimeSpan.FromHours(1)};

    // Act
    var result = location.GetTimeSlots(DayOfWeek.Tuesday, allowedDurations);

    // Assert
    var expected = new List<(TimeSpan, TimeSpan)>
    {
      (new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0))
    };

    Assert.Equal(expected, result);
  }

  #endregion
}
