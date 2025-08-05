using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class MovementNarrationRepositoryTests
{
  #region Fields

  private readonly string _expectedNarration = "You enter the bedroom.";
  private readonly Guid _movementId = Guid.NewGuid();
  private readonly MovementNarrationRepository _repository;

  #endregion

  #region Ctors

  #region Constructor

  public MovementNarrationRepositoryTests()
  {
    var narrationDataModels = new List<MovementNarrationDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        MovementId = _movementId,
        Text = _expectedNarration
      }
    };

    var context = A.Fake<ApplicationContext>();

    var narrationDbSet = narrationDataModels.BuildMockDbSet();

    A.CallTo(() => context.MovementNarrations).Returns(narrationDbSet);
    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new MovementNarrationRepository(context);
  }

  #endregion

  #endregion

  #region Tests

  [Fact]
  public async Task GetMovementNarrationFromMovementIdAsync_ShouldReturnNarration_WhenExists()
  {
    // Act
    var narration = await _repository.GetMovementNarrationFromMovementIdAsync(_movementId, CancellationToken.None);

    // Assert
    Assert.NotNull(narration);
    Assert.Equal(_expectedNarration, narration);
  }

  [Fact]
  public async Task GetMovementNarrationFromMovementIdAsync_ShouldThrow_WhenNarrationDoesNotExist()
  {
    // Arrange
    var nonExistentMovementId = Guid.NewGuid();

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _repository.GetMovementNarrationFromMovementIdAsync(nonExistentMovementId, CancellationToken.None)
    );

    Assert.Equal($"No narration found for movement {nonExistentMovementId}", ex.Message);
  }

  #endregion
}
