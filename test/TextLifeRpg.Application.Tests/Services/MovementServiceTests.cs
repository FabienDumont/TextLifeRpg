using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class MovementServiceTests
{
  #region Fields

  private readonly IMovementRepository _movementRepository = A.Fake<IMovementRepository>();
  private readonly IMovementNarrationRepository _movementNarrationRepository = A.Fake<IMovementNarrationRepository>();
  private readonly ILocationService _locationService = A.Fake<ILocationService>();
  private readonly MovementService _service;

  #endregion

  #region Ctors

  public MovementServiceTests()
  {
    _service = new MovementService(_movementRepository, _movementNarrationRepository, _locationService);
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

    A.CallTo(() => _movementRepository.GetMovementsAsync(currentLocationId, currentRoomId, A<CancellationToken>._))
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

    A.CallTo(() => _movementRepository.GetMovementsAsync(currentLocationId, currentRoomId, A<CancellationToken>._))
      .Throws(new InvalidOperationException("Failed to get movements"));

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetAvailableMovementsAsync(
        currentLocationId, currentRoomId, dayOfWeek, timeOfDay, CancellationToken.None
      )
    );

    Assert.Equal("Failed to get movements", ex.Message);
  }

  [Fact]
  public async Task ExecuteAsync_ShouldMovePlayerAdvanceTimeAndAddNarration()
  {
    // Arrange
    var movementId = Guid.NewGuid();
    var fromLocationId = Guid.NewGuid();
    var toLocationId = Guid.NewGuid();
    var toRoomId = Guid.NewGuid();

    var movement = Movement.Load(movementId, fromLocationId, null, toLocationId, toRoomId, null);

    var player = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player]);
    var save = GameSave.Create(player, world);

    var narrationText = "You walk into the next room.";

    A.CallTo(() =>
      _movementNarrationRepository.GetMovementNarrationFromMovementIdAsync(movementId, A<CancellationToken>._)
    ).Returns(narrationText);

    var originalTime = world.CurrentDate;

    // Act
    await _service.ExecuteAsync(movement, save, CancellationToken.None);

    // Assert
    Assert.Equal(toLocationId, player.LocationId);
    Assert.Equal(toRoomId, player.RoomId);
    Assert.True(world.CurrentDate > originalTime); // Time advanced

    var lastLine = save.TextLines.LastOrDefault();
    Assert.NotNull(lastLine);
    var fullNarration = string.Concat(lastLine.TextParts.Select(p => p.Text));
    Assert.Contains(narrationText, fullNarration);

    Assert.Single(save.TextLines);

    A.CallTo(() =>
      _movementNarrationRepository.GetMovementNarrationFromMovementIdAsync(movementId, A<CancellationToken>._)
    ).MustHaveHappenedOnceExactly();
  }

  #endregion
}
