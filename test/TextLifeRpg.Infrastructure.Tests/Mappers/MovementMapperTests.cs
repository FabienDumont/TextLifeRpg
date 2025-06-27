using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class MovementMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var fromLocationId = Guid.NewGuid();
    var fromRoomId = Guid.NewGuid();
    var toLocationId = Guid.NewGuid();
    var toRoomId = Guid.NewGuid();
    var requiredItemId = Guid.NewGuid();

    var dataModel = new MovementDataModel
    {
      Id = id,
      FromLocationId = fromLocationId,
      FromRoomId = fromRoomId,
      ToLocationId = toLocationId,
      ToRoomId = toRoomId,
      RequiredItemId = requiredItemId
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(fromLocationId, domain.FromLocationId);
    Assert.Equal(fromRoomId, domain.FromRoomId);
    Assert.Equal(toLocationId, domain.ToLocationId);
    Assert.Equal(toRoomId, domain.ToRoomId);
    Assert.Equal(requiredItemId, domain.RequiredItemId);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<MovementDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        FromLocationId = Guid.NewGuid(),
        FromRoomId = Guid.NewGuid(),
        ToLocationId = Guid.NewGuid(),
        ToRoomId = Guid.NewGuid(),
        RequiredItemId = null
      },
      new()
      {
        Id = Guid.NewGuid(),
        FromLocationId = Guid.NewGuid(),
        FromRoomId = null,
        ToLocationId = Guid.NewGuid(),
        ToRoomId = null,
        RequiredItemId = Guid.NewGuid()
      }
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);
    for (var i = 0; i < domainModels.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domainModels[i].Id);
      Assert.Equal(dataModels[i].FromLocationId, domainModels[i].FromLocationId);
      Assert.Equal(dataModels[i].FromRoomId, domainModels[i].FromRoomId);
      Assert.Equal(dataModels[i].ToLocationId, domainModels[i].ToLocationId);
      Assert.Equal(dataModels[i].ToRoomId, domainModels[i].ToRoomId);
      Assert.Equal(dataModels[i].RequiredItemId, domainModels[i].RequiredItemId);
    }
  }

  [Fact]
  public void ToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var fromLocationId = Guid.NewGuid();
    var fromRoomId = Guid.NewGuid();
    var toLocationId = Guid.NewGuid();
    var toRoomId = Guid.NewGuid();
    var requiredItemId = Guid.NewGuid();

    var domain = Movement.Load(id, fromLocationId, fromRoomId, toLocationId, toRoomId, requiredItemId);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(fromLocationId, dataModel.FromLocationId);
    Assert.Equal(fromRoomId, dataModel.FromRoomId);
    Assert.Equal(toLocationId, dataModel.ToLocationId);
    Assert.Equal(toRoomId, dataModel.ToRoomId);
    Assert.Equal(requiredItemId, dataModel.RequiredItemId);
  }

  #endregion
}
