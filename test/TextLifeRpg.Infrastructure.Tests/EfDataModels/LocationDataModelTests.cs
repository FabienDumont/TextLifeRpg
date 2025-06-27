using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class LocationDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Home";

    // Act
    var location = new LocationDataModel
    {
      Id = id,
      Name = name
    };

    // Assert
    Assert.Equal(id, location.Id);
    Assert.Equal(name, location.Name);
  }

  #endregion
}
