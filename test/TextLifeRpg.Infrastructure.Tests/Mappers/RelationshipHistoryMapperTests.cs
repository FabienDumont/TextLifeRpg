using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class RelationshipHistoryMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapCorrectly()
  {
    // Arrange
    var first = new DateOnly(2023, 1, 1);
    var last = new DateOnly(2024, 1, 1);
    var dataModel = new RelationshipHistoryDataModel
    {
      FirstInteraction = first,
      LastInteraction = last
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(first, domain.FirstInteraction);
    Assert.Equal(last, domain.LastInteraction);
  }

  [Fact]
  public void ToDataModel_ShouldMapCorrectly()
  {
    // Arrange
    var first = new DateOnly(2020, 5, 15);
    var last = new DateOnly(2025, 5, 15);
    var domain = RelationshipHistory.Create(first, last);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(first, dataModel.FirstInteraction);
    Assert.Equal(last, dataModel.LastInteraction);
  }

  #endregion
}
