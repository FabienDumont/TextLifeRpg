using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Tests.JsonDataModels;

public class RelationshipDataModelTests
{
  #region Methods

  [Fact]
  public void RelationshipDataModel_Should_InitializeWithDefaultValues()
  {
    // Act
    var model = new RelationshipDataModel
    {
      History = new RelationshipHistoryDataModel()
    };

    // Assert
    Assert.Equal(Guid.Empty, model.Id);
    Assert.Equal(Guid.Empty, model.SourceCharacterId);
    Assert.Equal(Guid.Empty, model.TargetCharacterId);
    Assert.Equal(0, model.Value);
    Assert.Equal(default, model.Type);
    Assert.NotNull(model.History);
  }

  [Fact]
  public void RelationshipDataModel_Should_AssignValuesCorrectly()
  {
    // Arrange
    var id = Guid.NewGuid();
    var sourceId = Guid.NewGuid();
    var targetId = Guid.NewGuid();
    var type = RelationshipType.Friend;
    var value = 42;
    var history = new RelationshipHistoryDataModel
    {
      FirstInteraction = new DateOnly(2024, 1, 1),
      LastInteraction = new DateOnly(2025, 1, 1)
    };

    // Act
    var model = new RelationshipDataModel
    {
      Id = id,
      SourceCharacterId = sourceId,
      TargetCharacterId = targetId,
      Type = type,
      Value = value,
      History = history
    };

    // Assert
    Assert.Equal(id, model.Id);
    Assert.Equal(sourceId, model.SourceCharacterId);
    Assert.Equal(targetId, model.TargetCharacterId);
    Assert.Equal(type, model.Type);
    Assert.Equal(value, model.Value);
    Assert.Same(history, model.History);
  }

  #endregion
}
