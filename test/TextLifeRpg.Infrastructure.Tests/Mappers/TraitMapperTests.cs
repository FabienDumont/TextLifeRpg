using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class TraitMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Curious";

    var dataModel = new TraitDataModel
    {
      Id = id,
      Name = name
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(name, domain.Name);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<TraitDataModel>
    {
      new() {Id = Guid.NewGuid(), Name = "Kind"},
      new() {Id = Guid.NewGuid(), Name = "Aggressive"}
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
    const string name = "Brave";

    var domain = Trait.Load(id, name);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(name, dataModel.Name);
  }

  #endregion
}
