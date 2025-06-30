using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class CharacterTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Act
    const string name = "Player";
    var birthDate = new DateOnly(1990, 1, 1);
    const BiologicalSex biologicalSex = BiologicalSex.Male;
    const int height = 180;
    const int weight = 70;
    const int muscleMass = 5;
    var attributes = CharacterAttributes.Create(5, 5, 5);
    var character = Character.Create(name, birthDate, biologicalSex, height, weight, muscleMass, attributes);

    // Assert
    Assert.NotNull(character);
    Assert.NotEqual(Guid.Empty, character.Id);
    Assert.Equal(name, character.Name);
    Assert.Equal(biologicalSex, character.BiologicalSex);
    Assert.Equal(height, character.Height);
    Assert.Equal(weight, character.Weight);
    Assert.Equal(muscleMass, character.MuscleMass);
    Assert.Equal(attributes, character.Attributes);
    Assert.Null(character.LocationId);
    Assert.Null(character.RoomId);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Player";
    var birthDate = new DateOnly(1990, 1, 1);
    const BiologicalSex biologicalSex = BiologicalSex.Male;
    const int height = 180;
    const int weight = 70;
    const int muscleMass = 5;
    var attributes = CharacterAttributes.Create(5, 5, 5);

    // Act
    var character = Character.Load(id, name, birthDate, biologicalSex, height, weight, muscleMass, attributes);

    // Assert
    Assert.Equal(id, character.Id);
    Assert.Equal(name, character.Name);
    Assert.Equal(biologicalSex, character.BiologicalSex);
    Assert.Equal(height, character.Height);
    Assert.Equal(weight, character.Weight);
    Assert.Equal(muscleMass, character.MuscleMass);
    Assert.Equal(attributes, character.Attributes);
    Assert.Null(character.LocationId);
    Assert.Null(character.RoomId);
  }

  [Fact]
  public void AddTraits_ShouldAddGivenTraitIds()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var trait1 = Guid.NewGuid();
    var trait2 = Guid.NewGuid();
    var traits = new List<Guid> {trait1, trait2};

    // Act
    character.AddTraits(traits);

    // Assert
    Assert.Equal(2, character.TraitsId.Count);
    Assert.Contains(trait1, character.TraitsId);
    Assert.Contains(trait2, character.TraitsId);
  }

  [Fact]
  public void MoveTo_ShouldChangeLocationAndRoom()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();

    // Act
    character.MoveTo(locationId, roomId);

    // Assert
    Assert.Equal(locationId, character.LocationId);
    Assert.Equal(roomId, character.RoomId);
  }

  [Theory]
  [InlineData(1990, 6, 15, 2025, 6, 15, 35)] // Birthday today
  [InlineData(1990, 6, 15, 2025, 6, 14, 34)] // Birthday tomorrow
  [InlineData(1990, 6, 15, 2025, 6, 16, 35)] // Birthday passed
  [InlineData(2000, 1, 1, 2000, 1, 1, 0)] // Born today
  public void GetAge_ShouldReturnCorrectAge(
    int birthYear, int birthMonth, int birthDay, int currentYear, int currentMonth, int currentDay, int expectedAge
  )
  {
    // Arrange
    var birthDate = new DateOnly(birthYear, birthMonth, birthDay);
    var gameDate = new DateOnly(currentYear, currentMonth, currentDay);
    var character = new CharacterBuilder().WithBirthDate(birthDate).Build();

    // Act
    var age = character.GetAge(gameDate);

    // Assert
    Assert.Equal(expectedAge, age);
  }

  #endregion
}
