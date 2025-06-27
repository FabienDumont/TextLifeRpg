using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class LocationMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Home";

    var dataModel = new LocationDataModel
    {
      Id = id,
      Name = name
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(name, domain.Name);
    Assert.True(domain.IsAlwaysOpen);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<LocationDataModel>
    {
      new() {Id = Guid.NewGuid(), Name = "Home"},
      new() {Id = Guid.NewGuid(), Name = "Street"}
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);

    Assert.Equal(dataModels[0].Id, domainModels[0].Id);
    Assert.Equal(dataModels[0].Name, domainModels[0].Name);

    Assert.Equal(dataModels[1].Id, domainModels[1].Id);
    Assert.Equal(dataModels[1].Name, domainModels[1].Name);
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Home";

    var domain = Location.Load(id, name, []);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(name, dataModel.Name);
  }

  #endregion
}
