using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class ItemMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();

    var dataModel = new ItemDataModel
    {
      Id = id,
      Name = string.Empty
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(string.Empty, domain.Name);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<ItemDataModel>
    {
      new() {Id = Guid.NewGuid(), Name = string.Empty},
      new() {Id = Guid.NewGuid(), Name = string.Empty}
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

    var domain = Item.Load(id, string.Empty);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(string.Empty, dataModel.Name);
  }

  #endregion
}
