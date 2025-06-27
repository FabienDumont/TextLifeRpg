using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class ExplorationActionMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    const string label = "Sleep";
    const int neededMinutes = 480;

    var dataModel = new ExplorationActionDataModel
    {
      Id = id,
      LocationId = locationId,
      RoomId = roomId,
      Label = label,
      NeededMinutes = neededMinutes
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.NotNull(domain);
    Assert.Equal(id, domain.Id);
    Assert.Equal(locationId, domain.LocationId);
    Assert.Equal(roomId, domain.RoomId);
    Assert.Equal(label, domain.Label);
    Assert.Equal(neededMinutes, domain.NeededMinutes);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<ExplorationActionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = Guid.NewGuid(),
        RoomId = Guid.NewGuid(),
        Label = "Sleep",
        NeededMinutes = 480
      },
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = Guid.NewGuid(),
        RoomId = null,
        Label = "Nap",
        NeededMinutes = 60
      }
    };

    // Act
    var domains = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domains.Count);
    for (var i = 0; i < domains.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domains[i].Id);
      Assert.Equal(dataModels[i].LocationId, domains[i].LocationId);
      Assert.Equal(dataModels[i].RoomId, domains[i].RoomId);
      Assert.Equal(dataModels[i].Label, domains[i].Label);
      Assert.Equal(dataModels[i].NeededMinutes, domains[i].NeededMinutes);
    }
  }

  [Fact]
  public void ToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    const string label = "Sleep";
    const int neededMinutes = 480;

    var domain = ExplorationAction.Load(id, locationId, roomId, label, neededMinutes);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.NotNull(dataModel);
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(locationId, dataModel.LocationId);
    Assert.Equal(roomId, dataModel.RoomId);
    Assert.Equal(label, dataModel.Label);
    Assert.Equal(neededMinutes, dataModel.NeededMinutes);
  }

  [Fact]
  public void ToDataModelCollection_ShouldMapDomainCollectionToDataModelCollection()
  {
    // Arrange
    var domains = new List<ExplorationAction>
    {
      ExplorationAction.Load(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Sleep", 480),
      ExplorationAction.Load(Guid.NewGuid(), Guid.NewGuid(), null, "Nap", 60)
    };

    // Act
    var dataModels = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(2, dataModels.Count);
    for (var i = 0; i < dataModels.Count; i++)
    {
      Assert.Equal(domains[i].Id, dataModels[i].Id);
      Assert.Equal(domains[i].LocationId, dataModels[i].LocationId);
      Assert.Equal(domains[i].RoomId, dataModels[i].RoomId);
      Assert.Equal(domains[i].Label, dataModels[i].Label);
      Assert.Equal(domains[i].NeededMinutes, dataModels[i].NeededMinutes);
    }
  }

  #endregion
}
