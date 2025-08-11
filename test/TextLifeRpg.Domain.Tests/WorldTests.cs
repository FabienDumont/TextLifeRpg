using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class WorldTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Arrange
    var currentDate = new DateTime(2025, 4, 24, 8, 0, 0);
    var characters = new List<Character>();

    // Act
    var world = World.Create(currentDate, characters);

    // Assert
    Assert.NotNull(world);
    Assert.NotEqual(Guid.Empty, world.Id);
    Assert.Equal(currentDate, world.CurrentDate);
    Assert.Same(characters, world.Characters);
    Assert.NotNull(world.Relationships);
    Assert.Empty(world.Relationships);
    Assert.NotNull(world.Schedules);
    Assert.Empty(world.Schedules);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var currentDate = new DateTime(2025, 4, 24, 8, 0, 0);
    var characters = new List<Character>();

    // Act
    var world = World.Load(id, currentDate, characters, [], []);

    // Assert
    Assert.Equal(id, world.Id);
    Assert.Equal(currentDate, world.CurrentDate);
    Assert.Same(characters, world.Characters);
  }

  [Fact]
  public void AddCharacter_ShouldAddCharacterToWorld()
  {
    // Arrange
    var world = World.Create(DateTime.UtcNow, []);
    var character = new CharacterBuilder().Build();

    // Act
    world.AddCharacter(character);

    // Assert
    Assert.Contains(character, world.Characters);
  }

  [Fact]
  public void AdvanceTime_ShouldUpdateCurrentDate()
  {
    // Arrange
    var initialDate = new DateTime(2025, 4, 24, 8, 0, 0);
    var world = World.Create(initialDate, []);
    const int minutes = 120;

    // Act
    world.AdvanceTime(minutes, Guid.NewGuid());

    // Assert
    Assert.Equal(initialDate.AddMinutes(minutes), world.CurrentDate);
  }

  [Fact]
  public void RefreshCharactersLocation_ShouldUpdateLocationBasedOnSchedule()
  {
    // Arrange
    var npc = new CharacterBuilder().Build();
    var player = new CharacterBuilder().Build();

    var currentDate = new DateTime(2025, 4, 24, 8, 30, 0); // Thursday 08:30
    var world = World.Create(currentDate, [player, npc]);

    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();

    var entry = new ScheduleEntry(DayOfWeek.Thursday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), locationId, roomId);

    var schedule = Schedule.Create(npc.Id, [entry]);
    world.SetSchedules([schedule]);

    // Act
    world.RefreshCharactersLocation(player.Id);

    // Assert
    Assert.Equal(locationId, npc.LocationId);
    Assert.Equal(roomId, npc.RoomId);
  }

  [Fact]
  public void RefreshCharactersLocation_ShouldClearLocation_WhenNoScheduleExists()
  {
    // Arrange
    var npc = new CharacterBuilder().Build();
    var player = new CharacterBuilder().Build();

    var currentDate = new DateTime(2025, 4, 24, 8, 30, 0);
    var world = World.Create(currentDate, [player, npc]);

    // No schedules set

    // Act
    world.RefreshCharactersLocation(player.Id);

    // Assert
    Assert.Null(npc.LocationId);
    Assert.Null(npc.RoomId);
  }

  #endregion
}
