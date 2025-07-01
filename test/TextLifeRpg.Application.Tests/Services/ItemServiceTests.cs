using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class ItemServiceTests
{
  #region Fields

  private readonly IItemRepository _itemRepository = A.Fake<IItemRepository>();
  private readonly ItemService _itemService;

  #endregion

  #region Ctors

  public ItemServiceTests()
  {
    _itemService = new ItemService(_itemRepository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetByIdAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var itemId = Guid.NewGuid();
    var item = Item.Load(itemId, string.Empty);
    A.CallTo(() => _itemRepository.GetByIdAsync(itemId, A<CancellationToken>._)).Returns(item);

    // Act
    var result = await _itemService.GetByIdAsync(itemId, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(itemId, result.Id);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var itemId = Guid.NewGuid();
    var item = Item.Load(itemId, string.Empty);
    A.CallTo(() => _itemRepository.GetByNameAsync(string.Empty, A<CancellationToken>._)).Returns(item);

    // Act
    var result = await _itemService.GetByNameAsync(string.Empty, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(itemId, result.Id);
  }

  [Fact]
  public async Task GetAllItemsAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var items = new List<Item> {Item.Load(Guid.NewGuid(), string.Empty)};
    A.CallTo(() => _itemRepository.GetAllAsync(A<CancellationToken>._)).Returns(items);

    // Act
    var result = await _itemService.GetAllAsync(CancellationToken.None);
    var resultList = result.ToList();

    // Assert
    Assert.Equal(items.Count, resultList.Count);
    Assert.Equal(items[0].Id, resultList[0].Id);
  }

  #endregion
}
