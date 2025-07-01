using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class ItemRepositoryTests
{
  #region Fields

  private readonly List<ItemDataModel> _items;
  private readonly ItemRepository _repository;

  #endregion

  #region Ctors

  public ItemRepositoryTests()
  {
    var context = A.Fake<ApplicationContext>();

    _items =
    [
      new ItemDataModel {Id = Guid.NewGuid(), Name = "Key"},
      new ItemDataModel {Id = Guid.NewGuid(), Name = "Badge"}
    ];

    var mockDbSet = _items.AsQueryable().BuildMockDbSet();
    A.CallTo(() => context.Items).Returns(mockDbSet);

    _repository = new ItemRepository(context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetById_ShouldReturnMappedItem_WhenItExists()
  {
    // Arrange
    var existing = _items[1];

    // Act
    var result = await _repository.GetByIdAsync(existing.Id, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(existing.Id, result.Id);
    Assert.Equal(existing.Name, result.Name);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldReturnMappedItem_WhenExists()
  {
    // Arrange
    var target = _items[1];

    // Act
    var result = await _repository.GetByNameAsync(target.Name, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(target.Id, result.Id);
    Assert.Equal(target.Name, result.Name);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldReturnNull_WhenNotFound()
  {
    // Act
    var result = await _repository.GetByNameAsync("Nothing", CancellationToken.None);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnMappedItems()
  {
    var result = await _repository.GetAllAsync(CancellationToken.None);

    Assert.NotNull(result);
    Assert.Equal(_items.Count, result.Count);
    foreach (var item in _items)
    {
      Assert.Contains(result, r => r.Id == item.Id);
    }
  }

  #endregion
}
