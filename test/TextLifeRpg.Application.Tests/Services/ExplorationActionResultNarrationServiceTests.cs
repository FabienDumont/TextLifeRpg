using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public sealed class ExplorationActionResultNarrationServiceTests
{
  #region Methods

  [Fact]
  public async Task GetExplorationActionResultNarrationAsync_ShouldReturnNarration()
  {
    // Arrange
    var narrationId = Guid.NewGuid();
    var resultId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 50;
    var world = World.Create(DateTime.Now, [character]);

    var expectedNarration = ExplorationActionResultNarration.Load(
      narrationId, resultId, "You're tired but manage to pull through."
    );

    var repo = A.Fake<IExplorationActionResultNarrationRepository>();
    A.CallTo(() => repo.GetByExplorationActionResultIdAsync(resultId, A<GameContext>._, A<CancellationToken>._))
      .Returns(expectedNarration);

    var service = new ExplorationActionResultNarrationService(repo);

    // Act
    var result = await service.GetExplorationActionResultNarrationAsync(
      resultId, character, world, CancellationToken.None
    );

    // Assert
    Assert.Equal(expectedNarration, result);
  }

  #endregion
}
