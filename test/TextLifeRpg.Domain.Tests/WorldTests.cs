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
    world.AdvanceTime(minutes);

    // Assert
    Assert.Equal(initialDate.AddMinutes(minutes), world.CurrentDate);
  }

  #endregion
}
