using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Tests.JsonDataModels;

public class GameSaveDataModelTests
{
  #region Methods

  [Fact]
  public void GameSaveDataModel_Should_InitializeWithDefaultValues()
  {
    // Act
    var model = new GameSaveDataModel
    {
      World = new WorldDataModel()
    };

    // Assert
    Assert.Equal(Guid.Empty, model.Id);
    Assert.Equal(string.Empty, model.Name);
    Assert.Equal(Guid.Empty, model.PlayerCharacterId);
    Assert.NotNull(model.World);
    Assert.Equal(default, model.SavedAt);
  }

  [Fact]
  public void GameSaveDataModel_Should_Assign_Values_Properly()
  {
    // Arrange
    var id = Guid.NewGuid();
    var characterId = Guid.NewGuid();
    var timestamp = DateTime.UtcNow;

    var character = new CharacterDataModel
    {
      Id = characterId,
      Name = "Test",
      TraitsId = [Guid.NewGuid()]
    };

    var worldDataModel = new WorldDataModel
    {
      Id = Guid.NewGuid(),
      CurrentDate = DateTime.Now,
      Characters = [character]
    };

    // Act
    var model = new GameSaveDataModel
    {
      Id = id,
      Name = "Test Save",
      PlayerCharacterId = characterId,
      World = worldDataModel,
      SavedAt = timestamp
    };

    // Assert
    Assert.Equal(id, model.Id);
    Assert.Equal("Test Save", model.Name);
    Assert.Equal(characterId, model.PlayerCharacterId);
    Assert.Equal(worldDataModel, model.World); // reference equality, not deep
    Assert.Equal(timestamp, model.SavedAt);
  }

  #endregion
}
