namespace TextLifeRpg.Domain.Tests;

public class InventoryEntryTests
{
  [Fact]
  public void Create_ShouldInitialize()
  {
    // Arrange
    var itemId = Guid.NewGuid();

    // Act
    var domain = InventoryEntry.Create(itemId, int.MinValue);

    // Assert
    Assert.NotNull(domain);
    Assert.Equal(itemId, domain.ItemId);
    Assert.Equal(int.MinValue, domain.Quantity);
  }

  [Fact]
  public void Add_ShouldIncreaseQuantity()
  {
    // Arrange
    var domain = InventoryEntry.Create(Guid.NewGuid(), 2);

    // Act
    domain.Add(3);

    // Assert
    Assert.Equal(5, domain.Quantity);
  }

  [Fact]
  public void Remove_ShouldDecreaseQuantity()
  {
    // Arrange
    var domain = InventoryEntry.Create(Guid.NewGuid(), 5);

    // Act
    domain.Remove(3);

    // Assert
    Assert.Equal(2, domain.Quantity);
  }
}
