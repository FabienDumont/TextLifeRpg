using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class TraitTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Act
    var trait = Trait.Create(string.Empty);

    // Assert
    Assert.NotNull(trait);
    Assert.NotEqual(Guid.Empty, trait.Id);
    Assert.Equal(string.Empty, trait.Name);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string name = "careful";

    // Act
    var trait = Trait.Load(id, name);

    // Assert
    Assert.Equal(id, trait.Id);
    Assert.Equal(name, trait.Name);
  }

  #endregion
}
