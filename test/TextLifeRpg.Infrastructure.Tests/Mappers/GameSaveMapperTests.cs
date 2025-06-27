using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class GameSaveMapperTests
{
  #region Methods

  [Fact]
  public void ToDataModel_Should_Map_GameSave_To_GameSaveDataModel()
  {
    // Arrange
    var playerCharacter = new CharacterBuilder().Build();
    playerCharacter.AddTraits(new List<Guid> {Guid.NewGuid(), Guid.NewGuid()});
    var world = World.Create(DateTime.Now, [playerCharacter]);
    var save = GameSave.Create(playerCharacter, world);

    // Act
    var dataModel = save.ToDataModel();

    // Assert
    Assert.Equal(save.Id, dataModel.Id);
    Assert.Equal(save.Name, dataModel.Name);
    Assert.Equal(save.PlayerCharacterId, dataModel.PlayerCharacterId);
    Assert.NotNull(dataModel.World);
  }

  [Fact]
  public void ToDomain_Should_Map_GameSaveDataModel_To_GameSave()
  {
    // Arrange
    var characterId = Guid.NewGuid();
    var traitIds = new List<Guid> {Guid.NewGuid()};

    var worldDataModel = new WorldDataModel
    {
      Id = Guid.NewGuid(),
      CurrentDate = DateTime.Now,
      Characters =
      [
        new CharacterDataModel
        {
          Id = characterId,
          Name = "Player",
          TraitsId = traitIds
        }
      ]
    };

    var dataModel = new GameSaveDataModel
    {
      Id = Guid.NewGuid(),
      Name = $"Player_{worldDataModel.CurrentDate:yyyyMMdd_HHmmss}",
      World = worldDataModel,
      PlayerCharacterId = characterId,
      SavedAt = DateTime.UtcNow
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(dataModel.Id, domain.Id);
    Assert.Equal(dataModel.Name, domain.Name);
    Assert.Equal(dataModel.PlayerCharacterId, domain.PlayerCharacterId);
    Assert.NotNull(domain.World);
    Assert.Equal(characterId, domain.PlayerCharacter.Id);
  }

  [Fact]
  public void ToDomainCollection_Should_Map_List_Of_DataModels()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    var id2 = Guid.NewGuid();

    var world1DataModel = new WorldDataModel
    {
      Id = Guid.NewGuid(),
      CurrentDate = new DateTime(2025, 1, 1),
      Characters = [new CharacterDataModel {Id = id1, Name = "CharA"}]
    };

    var world2DataModel = new WorldDataModel
    {
      Id = Guid.NewGuid(),
      CurrentDate = new DateTime(2025, 1, 2),
      Characters = [new CharacterDataModel {Id = id2, Name = "CharB"}]
    };

    var dataModels = new List<GameSaveDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        Name = "Save A",
        PlayerCharacterId = id1,
        World = world1DataModel
      },
      new()
      {
        Id = Guid.NewGuid(),
        Name = "Save B",
        PlayerCharacterId = id2,
        World = world2DataModel
      }
    };

    // Act
    var domainSaves = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainSaves.Count);
    Assert.Equal($"CharA_{world1DataModel.CurrentDate:yyyyMMdd_HHmmss}", domainSaves[0].Name);
    Assert.Equal($"CharB_{world2DataModel.CurrentDate:yyyyMMdd_HHmmss}", domainSaves[1].Name);
  }

  #endregion
}
