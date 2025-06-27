using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class TraitIncompatibilityDataModelTests
{
  #region Methods

  [Fact]
  public void Instanciation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var trait1Id = Guid.NewGuid();
    var trait2Id = Guid.NewGuid();

    var trait1 = new TraitDataModel
    {
      Id = trait1Id,
      Name = string.Empty
    };

    var trait2 = new TraitDataModel
    {
      Id = trait2Id,
      Name = string.Empty
    };

    // Act
    var dataModel = new TraitIncompatibilityDataModel
    {
      Trait1Id = trait1Id,
      Trait2Id = trait2Id,
      Trait1 = trait1,
      Trait2 = trait2
    };

    // Assert
    Assert.Equal(trait1Id, dataModel.Trait1Id);
    Assert.Equal(trait2Id, dataModel.Trait2Id);
    Assert.Equal(trait1, dataModel.Trait1);
    Assert.Equal(trait2, dataModel.Trait2);
  }

  #endregion
}
