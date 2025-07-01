using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class ItemDataModelTests
{
  #region Methods

  [Fact]
  public void Instanciation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();

    // Act
    var dataModel = new ItemDataModel
    {
      Id = id,
      Name = string.Empty
    };

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(string.Empty, dataModel.Name);
  }

  #endregion
}
