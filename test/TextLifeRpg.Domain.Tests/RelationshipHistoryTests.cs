namespace TextLifeRpg.Domain.Tests;

public class RelationshipHistoryTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithProvidedValues()
  {
    // Arrange
    var first = new DateOnly(1990, 1, 1);
    var last = new DateOnly(2024, 5, 1);

    // Act
    var history = RelationshipHistory.Create(first, last);

    // Assert
    Assert.Equal(first, history.FirstInteraction);
    Assert.Equal(last, history.LastInteraction);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var first = new DateOnly(2023, 6, 15);
    var last = new DateOnly(2025, 1, 1);

    // Act
    var history = RelationshipHistory.Load(first, last);

    // Assert
    Assert.Equal(first, history.FirstInteraction);
    Assert.Equal(last, history.LastInteraction);
  }

  [Fact]
  public void UpdateLastInteraction_ShouldChangeLastInteraction()
  {
    // Arrange
    var first = new DateOnly(2024, 1, 1);
    var last = new DateOnly(2024, 5, 1);
    var updated = new DateOnly(2025, 3, 30);

    var history = RelationshipHistory.Create(first, last);

    // Act
    history.UpdateLastInteraction(updated);

    // Assert
    Assert.Equal(updated, history.LastInteraction);
  }

  #endregion
}
