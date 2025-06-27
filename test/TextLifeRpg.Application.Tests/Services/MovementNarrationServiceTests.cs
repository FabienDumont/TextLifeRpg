using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class MovementNarrationServiceTests
{
  #region Fields

  private readonly IMovementNarrationRepository _repository = A.Fake<IMovementNarrationRepository>();
  private readonly MovementNarrationService _service;

  #endregion

  #region Ctors

  public MovementNarrationServiceTests()
  {
    _service = new MovementNarrationService(_repository);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetNarrationTextAsync_ShouldReturnText_WhenNarrationExists()
  {
    // Arrange
    var movementId = Guid.NewGuid();
    const string expectedText = "You enter the bedroom.";
    var narration = MovementNarration.Load(Guid.NewGuid(), movementId, expectedText);

    A.CallTo(() => _repository.GetMovementNarrationFromMovementIdAsync(movementId, A<CancellationToken>._))
      .Returns(Task.FromResult(narration));

    // Act
    var result = await _service.GetNarrationTextAsync(movementId, CancellationToken.None);

    // Assert
    Assert.Equal(expectedText, result);
  }

  [Fact]
  public async Task GetNarrationTextAsync_ShouldThrow_WhenRepositoryThrows()
  {
    // Arrange
    var movementId = Guid.NewGuid();
    A.CallTo(() => _repository.GetMovementNarrationFromMovementIdAsync(movementId, A<CancellationToken>._))
      .Throws(new InvalidOperationException("Narration not found"));

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      _service.GetNarrationTextAsync(movementId, CancellationToken.None)
    );

    Assert.Equal("Narration not found", exception.Message);
  }

  #endregion
}
