using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class ExplorationActionResultMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var actionId = Guid.NewGuid();

    var dataModel = new ExplorationActionResultDataModel
    {
      Id = id,
      ExplorationActionId = actionId,
      AddMinutes = true,
      EnergyChange = 20,
      MoneyChange = -10
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.NotNull(domain);
    Assert.Equal(id, domain.Id);
    Assert.Equal(actionId, domain.ExplorationActionId);
    Assert.True(domain.AddMinutes);
    Assert.Equal(20, domain.EnergyChange);
    Assert.Equal(-10, domain.MoneyChange);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<ExplorationActionResultDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ExplorationActionId = Guid.NewGuid(),
        AddMinutes = true,
        EnergyChange = 10,
        MoneyChange = 0
      },
      new()
      {
        Id = Guid.NewGuid(),
        ExplorationActionId = Guid.NewGuid(),
        AddMinutes = false,
        EnergyChange = -5,
        MoneyChange = 5
      }
    };

    // Act
    var domains = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domains.Count);
    for (var i = 0; i < domains.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domains[i].Id);
      Assert.Equal(dataModels[i].ExplorationActionId, domains[i].ExplorationActionId);
      Assert.Equal(dataModels[i].AddMinutes, domains[i].AddMinutes);
      Assert.Equal(dataModels[i].EnergyChange, domains[i].EnergyChange);
      Assert.Equal(dataModels[i].MoneyChange, domains[i].MoneyChange);
    }
  }

  [Fact]
  public void ToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var actionId = Guid.NewGuid();

    var domain = ExplorationActionResult.Load(id, actionId, true, 20, -10);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.NotNull(dataModel);
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(actionId, dataModel.ExplorationActionId);
    Assert.True(dataModel.AddMinutes);
    Assert.Equal(20, dataModel.EnergyChange);
    Assert.Equal(-10, dataModel.MoneyChange);
  }

  [Fact]
  public void ToDataModelCollection_ShouldMapDomainCollectionToDataModelCollection()
  {
    // Arrange
    var domains = new List<ExplorationActionResult>
    {
      ExplorationActionResult.Load(Guid.NewGuid(), Guid.NewGuid(), true, 15, 0),
      ExplorationActionResult.Load(Guid.NewGuid(), Guid.NewGuid(), false, -5, 10)
    };

    // Act
    var dataModels = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(2, dataModels.Count);
    for (var i = 0; i < dataModels.Count; i++)
    {
      Assert.Equal(domains[i].Id, dataModels[i].Id);
      Assert.Equal(domains[i].ExplorationActionId, dataModels[i].ExplorationActionId);
      Assert.Equal(domains[i].AddMinutes, dataModels[i].AddMinutes);
      Assert.Equal(domains[i].EnergyChange, dataModels[i].EnergyChange);
      Assert.Equal(domains[i].MoneyChange, dataModels[i].MoneyChange);
    }
  }

  #endregion
}
