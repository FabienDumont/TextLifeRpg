using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class ExplorationActionResultServiceTests
{
  #region Fields

  private readonly IExplorationActionResultRepository _repository = A.Fake<IExplorationActionResultRepository>();
  private readonly ExplorationActionResultService _service;

  #endregion

  #region Ctors

  public ExplorationActionResultServiceTests()
  {
    _service = new ExplorationActionResultService(_repository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetExplorationActionResultAsync_ShouldReturnExpectedResult()
  {
    // Arrange
    var actionId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var expectedResult = ExplorationActionResult.Load(Guid.NewGuid(), actionId, true, 15, -10);

    A.CallTo(() => _repository.GetByExplorationActionIdAsync(actionId, A<GameContext>._, A<CancellationToken>._))
      .Returns(expectedResult);

    // Act
    var result = await _service.GetExplorationActionResultAsync(actionId, character, world, CancellationToken.None);

    // Assert
    Assert.Same(expectedResult, result);
    A.CallTo(() => _repository.GetByExplorationActionIdAsync(actionId, A<GameContext>._, A<CancellationToken>._))
      .MustHaveHappenedOnceExactly();
  }

  #endregion
}
