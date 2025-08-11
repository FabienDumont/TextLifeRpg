namespace TextLifeRpg.Domain.Tests;

public class RelationshipTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithCorrectValues()
  {
    // Arrange
    var sourceId = Guid.NewGuid();
    var targetId = Guid.NewGuid();
    var type = RelationshipType.Friend;
    var value = 60;
    var first = new DateOnly(2024, 1, 1);
    var last = new DateOnly(2024, 4, 1);

    // Act
    var relationship = Relationship.Create(sourceId, targetId, type, first, last, value);

    // Assert
    Assert.NotEqual(Guid.Empty, relationship.Id);
    Assert.Equal(sourceId, relationship.SourceCharacterId);
    Assert.Equal(targetId, relationship.TargetCharacterId);
    Assert.Equal(type, relationship.Type);
    Assert.Equal(value, relationship.Value);
    Assert.Equal(first, relationship.History.FirstInteraction);
    Assert.Equal(last, relationship.History.LastInteraction);
  }

  [Fact]
  public void Load_ShouldInitializeWithProvidedData()
  {
    // Arrange
    var id = Guid.NewGuid();
    var sourceId = Guid.NewGuid();
    var targetId = Guid.NewGuid();
    var type = RelationshipType.Enemy;
    var value = -75;
    var first = new DateOnly(2023, 6, 1);
    var last = new DateOnly(2025, 1, 1);
    var history = RelationshipHistory.Load(first, last);

    // Act
    var relationship = Relationship.Load(id, sourceId, targetId, value, type, history);

    // Assert
    Assert.Equal(id, relationship.Id);
    Assert.Equal(sourceId, relationship.SourceCharacterId);
    Assert.Equal(targetId, relationship.TargetCharacterId);
    Assert.Equal(type, relationship.Type);
    Assert.Equal(value, relationship.Value);
    Assert.Equal(first, relationship.History.FirstInteraction);
    Assert.Equal(last, relationship.History.LastInteraction);
  }

  [Fact]
  public void AdjustValue_ShouldIncreaseValueAndUpdateTime()
  {
    // Arrange
    var sourceId = Guid.NewGuid();
    var targetId = Guid.NewGuid();
    var first = new DateOnly(2024, 1, 1);
    var last = new DateOnly(2024, 5, 1);
    var newTime = new DateOnly(2025, 1, 1);

    var relationship = Relationship.Create(sourceId, targetId, RelationshipType.Acquaintance, first, last, 0);

    // Act
    relationship.AdjustValue(25, newTime);

    // Assert
    Assert.Equal(25, relationship.Value);
    Assert.Equal(newTime, relationship.History.LastInteraction);
    Assert.Equal(RelationshipType.Acquaintance, relationship.Type);
  }

  [Fact]
  public void AdjustValue_ShouldClampToMax()
  {
    // Arrange
    var rel = Relationship.Create(
      Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Acquaintance, new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1),
      95
    );

    // Act
    rel.AdjustValue(+10, new DateOnly(2024, 1, 1));

    // Assert
    Assert.Equal(100, rel.Value);
  }

  [Fact]
  public void AdjustValue_ShouldClampToMin()
  {
    // Arrange
    var rel = Relationship.Create(
      Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Acquaintance, new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1),
      -95
    );

    // Act
    rel.AdjustValue(-10, new DateOnly(2024, 1, 1));

    // Assert
    Assert.Equal(-100, rel.Value);
  }

  [Fact]
  public void AdjustValue_ShouldChangeTypeToFriend()
  {
    // Arrange
    var rel = Relationship.Create(
      Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Acquaintance, new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1),
      49
    );

    // Act
    rel.AdjustValue(+1, new DateOnly(2024, 1, 1));

    // Assert
    Assert.Equal(RelationshipType.Friend, rel.Type);
  }

  [Fact]
  public void AdjustValue_ShouldChangeTypeToEnemy()
  {
    // Arrange
    var rel = Relationship.Create(
      Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Friend, new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1), -49
    );

    // Act
    rel.AdjustValue(-1, new DateOnly(2024, 1, 1));

    // Assert
    Assert.Equal(RelationshipType.Enemy, rel.Type);
  }

  [Fact]
  public void AdjustValue_ShouldNotChangeTypeForFixedTypes()
  {
    // Arrange
    var rel = Relationship.Create(
      Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Parent, new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1), 0
    );

    // Act
    rel.AdjustValue(100, new DateOnly(2024, 1, 1));

    // Assert
    Assert.Equal(RelationshipType.Parent, rel.Type);
    Assert.Equal(100, rel.Value);
  }

  #endregion
}
