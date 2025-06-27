using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class MovementServiceTests
{
  #region Fields

  private readonly ILocationService _locationService = A.Fake<ILocationService>();
  private readonly IMovementRepository _repository = A.Fake<IMovementRepository>();
  private readonly MovementService _service;

  #endregion

  #region Ctors

  public MovementServiceTests()
  {
    _service = new MovementService(_repository, _locationService);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetAvailableMovementsAsync_ShouldReturnMovements_WhenMovementsExist()
  {
    // Arrange
    var currentLocationId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var destinationLocation1Id = Guid.NewGuid();
    var destinationLocation2Id = Guid.NewGuid();
    const DayOfWeek dayOfWeek = DayOfWeek.Monday;
    var timeOfDay = new TimeSpan(8, 0, 0);

    var expectedMovements = new List<Movement>
    {
      Movement.Load(Guid.NewGuid(), currentLocationId, currentRoomId, destinationLocation1Id, Guid.NewGuid(), null),
      Movement.Load(Guid.NewGuid(), currentLocationId, currentRoomId, destinationLocation2Id, null, null)
    };

    A.CallTo(() => _repository.GetMovementsAsync(currentLocationId, currentRoomId, A<CancellationToken>._))
      .Returns(Task.FromResult(expectedMovements));

    A.CallTo(() => _locationService.IsLocationOpenAsync(
        destinationLocation1Id, dayOfWeek, timeOfDay, CancellationToken.None
      )
    ).Returns(true);
    A.CallTo(() => _locationService.IsLocationOpenAsync(
        destinationLocation2Id, dayOfWeek, timeOfDay, CancellationToken.None
      )
    ).Returns(true);

    // Act
    var result = await _service.GetAvailableMovementsAsync(
      currentLocationId, currentRoomId, dayOfWeek, timeOfDay, CancellationToken.None
    );

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedMovements.Count, result.Count);
    Assert.All(expectedMovements, expected => Assert.Contains(expected, result));
  }

  [Fact]
  public async Task GetAvailableMovementsAsync_ShouldThrow_WhenRepositoryThrows()
  {
    // Arrange
    var currentLocationId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    const DayOfWeek dayOfWeek = DayOfWeek.Monday;
    var timeOfDay = new TimeSpan(8, 0, 0);

    A.CallTo(() => _repository.GetMovementsAsync(currentLocationId, currentRoomId, A<CancellationToken>._))
      .Throws(new InvalidOperationException("Failed to get movements"));

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetAvailableMovementsAsync(
        currentLocationId, currentRoomId, dayOfWeek, timeOfDay, CancellationToken.None
      )
    );

    Assert.Equal("Failed to get movements", ex.Message);
  }

  #endregion
}
