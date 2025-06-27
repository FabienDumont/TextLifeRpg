using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class RoomMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    const string name = "Bedroom";

    var dataModel = new RoomDataModel
    {
      Id = id,
      LocationId = locationId,
      Name = name
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(locationId, domain.LocationId);
    Assert.Equal(name, domain.Name);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<RoomDataModel>
    {
      new() {Id = Guid.NewGuid(), LocationId = Guid.NewGuid(), Name = "Bedroom"},
      new() {Id = Guid.NewGuid(), LocationId = Guid.NewGuid(), Name = "Living room"}
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);
    for (var i = 0; i < domainModels.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domainModels[i].Id);
      Assert.Equal(dataModels[i].LocationId, domainModels[i].LocationId);
      Assert.Equal(dataModels[i].Name, domainModels[i].Name);
    }
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    const string name = "Bedroom";

    var domain = Room.Load(id, locationId, name, true);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(name, dataModel.Name);
    Assert.True(dataModel.IsPlayerSpawn);
  }

  #endregion
}
