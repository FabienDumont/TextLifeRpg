using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class GreetingMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string spokenText = "Yo, what’s up?";

    var dataModel = new GreetingDataModel
    {
      Id = id,
      SpokenText = spokenText
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(spokenText, domain.SpokenText);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<GreetingDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        SpokenText = "Hey"
      },
      new()
      {
        Id = Guid.NewGuid(),
        SpokenText = "Hi there"
      }
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);

    for (var i = 0; i < domainModels.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domainModels[i].Id);
      Assert.Equal(dataModels[i].SpokenText, domainModels[i].SpokenText);
    }
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string spokenText = "Welcome, traveler.";

    var domain = Greeting.Load(id, spokenText);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(spokenText, dataModel.SpokenText);
  }

  #endregion
}
