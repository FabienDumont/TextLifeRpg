using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class InventoryEntryMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var itemId = Guid.NewGuid();
    var dataModel = new InventoryEntryDataModel
    {
      ItemId = itemId,
      Quantity = int.MinValue
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(itemId, domain.ItemId);
    Assert.Equal(int.MinValue, domain.Quantity);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var list = new List<InventoryEntryDataModel>
    {
      new() {ItemId = Guid.NewGuid(), Quantity = int.MinValue},
      new() {ItemId = Guid.NewGuid(), Quantity = int.MinValue}
    };

    // Act
    var result = list.ToDomainCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(list[0].ItemId, result[0].ItemId);
    Assert.Equal(list[0].Quantity, result[0].Quantity);
    Assert.Equal(list[1].ItemId, result[1].ItemId);
    Assert.Equal(list[1].Quantity, result[1].Quantity);
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var itemId = Guid.NewGuid();
    var domain = InventoryEntry.Create(itemId, int.MinValue);

    // Act
    var result = domain.ToDataModel();

    // Assert
    Assert.Equal(itemId, result.ItemId);
    Assert.Equal(int.MinValue, result.Quantity);
  }

  [Fact]
  public void MapToDataModelCollection_ShouldMapDomainCollectionToDataModelCollection()
  {
    // Arrange
    var domains = new List<InventoryEntry>
    {
      InventoryEntry.Create(Guid.NewGuid(), int.MinValue),
      InventoryEntry.Create(Guid.NewGuid(), int.MinValue)
    };

    // Act
    var result = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(domains[0].ItemId, result[0].ItemId);
    Assert.Equal(domains[0].Quantity, result[0].Quantity);
    Assert.Equal(domains[1].ItemId, result[1].ItemId);
    Assert.Equal(domains[1].Quantity, result[1].Quantity);
  }

  #endregion
}
