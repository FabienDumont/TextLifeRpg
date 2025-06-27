using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Tests.JsonDataModels;

public class RelationshipHistoryDataModelTests
{
  #region Methods

  [Fact]
  public void RelationshipHistoryDataModel_Should_InitializeWithDefaultValues()
  {
    // Act
    var model = new RelationshipHistoryDataModel();

    // Assert
    Assert.Equal(default, model.FirstInteraction);
    Assert.Equal(default, model.LastInteraction);
  }

  [Fact]
  public void RelationshipHistoryDataModel_Should_AssignValuesCorrectly()
  {
    // Arrange
    var first = new DateOnly(2023, 1, 1);
    var last = new DateOnly(2025, 4, 30);

    // Act
    var model = new RelationshipHistoryDataModel
    {
      FirstInteraction = first,
      LastInteraction = last
    };

    // Assert
    Assert.Equal(first, model.FirstInteraction);
    Assert.Equal(last, model.LastInteraction);
  }

  #endregion
}
