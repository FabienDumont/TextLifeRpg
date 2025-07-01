namespace TextLifeRpg.Domain.Tests;

public class ItemTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Act
    var item = Item.Create(string.Empty);

    // Assert
    Assert.NotNull(item);
    Assert.NotEqual(Guid.Empty, item.Id);
    Assert.Equal(string.Empty, item.Name);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();

    // Act
    var item = Item.Load(id, string.Empty);

    // Assert
    Assert.Equal(id, item.Id);
    Assert.Equal(string.Empty, item.Name);
  }

  #endregion
}
