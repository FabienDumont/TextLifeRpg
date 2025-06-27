using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class TraitDataModelTests
{
  #region Methods

  [Fact]
  public void Instanciation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "Brave";

    // Act
    var trait = new TraitDataModel
    {
      Id = id,
      Name = name
    };

    // Assert
    Assert.Equal(id, trait.Id);
    Assert.Equal(name, trait.Name);
  }

  #endregion
}
