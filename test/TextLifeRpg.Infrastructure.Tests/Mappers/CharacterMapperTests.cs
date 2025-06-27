using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class CharacterMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_Should_Map_CharacterDataModel_To_Character()
  {
    // Arrange
    var id = Guid.NewGuid();
    var trait1 = Guid.NewGuid();
    var trait2 = Guid.NewGuid();
    var dataModel = new CharacterDataModel
    {
      Id = id,
      Name = "Guts",
      TraitsId = [trait1, trait2]
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.NotNull(domain);
    Assert.Equal(dataModel.Id, domain.Id);
    Assert.Equal(dataModel.Name, domain.Name);
    Assert.Equal(2, domain.TraitsId.Count);
    Assert.Contains(trait1, domain.TraitsId);
    Assert.Contains(trait2, domain.TraitsId);
  }

  [Fact]
  public void ToDomainCollection_Should_Map_List_Of_DataModels()
  {
    // Arrange
    var list = new List<CharacterDataModel>
    {
      new() {Id = Guid.NewGuid(), Name = "A"},
      new() {Id = Guid.NewGuid(), Name = "B"}
    };

    // Act
    var result = list.ToDomainCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal("A", result[0].Name);
    Assert.Equal("B", result[1].Name);
  }

  [Fact]
  public void ToDataModel_Should_Map_Character_To_DataModel()
  {
    // Arrange
    var playerCharacter = new CharacterBuilder().Build();
    var traits = new List<Guid> {Guid.NewGuid(), Guid.NewGuid()};
    playerCharacter.AddTraits(traits);

    // Act
    var dataModel = playerCharacter.ToDataModel();

    // Assert
    Assert.NotNull(dataModel);
    Assert.Equal(playerCharacter.Id, dataModel.Id);
    Assert.Equal(playerCharacter.Name, dataModel.Name);
    Assert.Equal(2, dataModel.TraitsId.Count);
    Assert.Contains(traits[0], dataModel.TraitsId);
    Assert.Contains(traits[1], dataModel.TraitsId);
  }

  [Fact]
  public void ToDataModelCollection_Should_Map_List_Of_Characters()
  {
    // Arrange
    var c1 = new CharacterBuilder().Build();
    var c2 = new CharacterBuilder().Build();

    var list = new List<Character> {c1, c2};

    // Act
    var result = list.ToDataModelCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(c1.Name, result[0].Name);
    Assert.Equal(c2.Name, result[1].Name);
  }

  #endregion
}
