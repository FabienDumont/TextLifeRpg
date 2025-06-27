using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class RelationshipMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapCorrectly()
  {
    // Arrange
    var dataModel = new RelationshipDataModel
    {
      Id = Guid.NewGuid(),
      SourceCharacterId = Guid.NewGuid(),
      TargetCharacterId = Guid.NewGuid(),
      Value = 42,
      Type = RelationshipType.Friend,
      History = new RelationshipHistoryDataModel
      {
        FirstInteraction = new DateOnly(2023, 1, 1),
        LastInteraction = new DateOnly(2024, 1, 1)
      }
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(dataModel.Id, domain.Id);
    Assert.Equal(dataModel.SourceCharacterId, domain.SourceCharacterId);
    Assert.Equal(dataModel.TargetCharacterId, domain.TargetCharacterId);
    Assert.Equal(dataModel.Value, domain.Value);
    Assert.Equal(dataModel.Type, domain.Type);
    Assert.Equal(dataModel.History.FirstInteraction, domain.History.FirstInteraction);
    Assert.Equal(dataModel.History.LastInteraction, domain.History.LastInteraction);
  }

  [Fact]
  public void ToDataModel_ShouldMapCorrectly()
  {
    // Arrange
    var history = RelationshipHistory.Create(new DateOnly(2022, 6, 1), new DateOnly(2024, 6, 1));

    var domain = Relationship.Load(
      Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), -50, RelationshipType.Enemy, history
    );

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(domain.Id, dataModel.Id);
    Assert.Equal(domain.SourceCharacterId, dataModel.SourceCharacterId);
    Assert.Equal(domain.TargetCharacterId, dataModel.TargetCharacterId);
    Assert.Equal(domain.Value, dataModel.Value);
    Assert.Equal(domain.Type, dataModel.Type);
    Assert.Equal(history.FirstInteraction, dataModel.History.FirstInteraction);
    Assert.Equal(history.LastInteraction, dataModel.History.LastInteraction);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapAllItems()
  {
    // Arrange
    var dataModels = new List<RelationshipDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        SourceCharacterId = Guid.NewGuid(),
        TargetCharacterId = Guid.NewGuid(),
        Value = 0,
        Type = RelationshipType.Acquaintance,
        History = new RelationshipHistoryDataModel
        {
          FirstInteraction = new DateOnly(2023, 1, 1),
          LastInteraction = new DateOnly(2023, 6, 1)
        }
      },
      new()
      {
        Id = Guid.NewGuid(),
        SourceCharacterId = Guid.NewGuid(),
        TargetCharacterId = Guid.NewGuid(),
        Value = 100,
        Type = RelationshipType.Friend,
        History = new RelationshipHistoryDataModel
        {
          FirstInteraction = new DateOnly(2020, 1, 1),
          LastInteraction = new DateOnly(2024, 1, 1)
        }
      }
    };

    // Act
    var result = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(dataModels[0].Id, result[0].Id);
    Assert.Equal(dataModels[1].Id, result[1].Id);
  }

  [Fact]
  public void ToDataModelCollection_ShouldMapAllItems()
  {
    // Arrange
    var domains = new List<Relationship>
    {
      Relationship.Create(
        Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Friend, new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1), 20
      ),
      Relationship.Create(
        Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Enemy, new DateOnly(2021, 1, 1), new DateOnly(2024, 1, 1), -80
      )
    };

    // Act
    var result = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(domains[0].Id, result[0].Id);
    Assert.Equal(domains[1].Id, result[1].Id);
  }

  #endregion
}
