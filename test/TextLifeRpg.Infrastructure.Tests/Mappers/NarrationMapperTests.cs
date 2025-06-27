using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class NarrationMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var key = "intro_scene";
    var text = "You wake up in a strange room.";

    var dataModel = new NarrationDataModel
    {
      Id = id,
      Key = key,
      Text = text
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(key, domain.Key);
    Assert.Equal(text, domain.Text);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<NarrationDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        Key = "scene_1",
        Text = "You step outside."
      },
      new()
      {
        Id = Guid.NewGuid(),
        Key = "scene_2",
        Text = "You return to your home."
      }
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);
    for (var i = 0; i < domainModels.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domainModels[i].Id);
      Assert.Equal(dataModels[i].Key, domainModels[i].Key);
      Assert.Equal(dataModels[i].Text, domainModels[i].Text);
    }
  }

  [Fact]
  public void ToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var key = "alley_scene";
    var text = "You sneak into the alley.";

    var domain = Narration.Load(id, key, text);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(key, dataModel.Key);
    Assert.Equal(text, dataModel.Text);
  }

  #endregion
}
