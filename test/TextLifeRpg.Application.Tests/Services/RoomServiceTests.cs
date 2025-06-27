using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class RoomServiceTests
{
  #region Fields

  private readonly IRoomRepository _repository = A.Fake<IRoomRepository>();
  private readonly RoomService _service;

  #endregion

  #region Ctors

  public RoomServiceTests()
  {
    _service = new RoomService(_repository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetByIdAsync_ShouldReturnRoom_WhenRoomExists()
  {
    // Arrange
    var expectedRoom = Room.Load(Guid.NewGuid(), Guid.NewGuid(), "Kitchen", false);

    A.CallTo(() => _repository.GetByIdAsync(expectedRoom.Id, A<CancellationToken>._))
      .Returns(Task.FromResult(expectedRoom));

    // Act
    var result = await _service.GetByIdAsync(expectedRoom.Id, CancellationToken.None);

    // Assert
    Assert.Same(expectedRoom, result);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldThrow_WhenRepositoryThrows()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var expectedMessage = $"Room with ID {roomId} was not found.";

    A.CallTo(() => _repository.GetByIdAsync(roomId, A<CancellationToken>._))
      .Throws(new InvalidOperationException(expectedMessage));

    // Act
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetByIdAsync(
        roomId, CancellationToken.None
      )
    );

    // Assert
    Assert.Equal(expectedMessage, ex.Message);
  }

  [Fact]
  public async Task GetPlayerSpawnAsync_ShouldReturnRoom_WhenEntryPointExists()
  {
    // Arrange
    var locationId = Guid.NewGuid();
    var expectedRoom = Room.Load(Guid.NewGuid(), locationId, "Bedroom", true);

    A.CallTo(() => _repository.GetPlayerSpawnAsync(A<CancellationToken>._))
      .Returns(Task.FromResult<Room?>(expectedRoom));

    // Act
    var result = await _service.GetPlayerSpawnAsync(CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Same(expectedRoom, result);
  }

  [Fact]
  public async Task GetPlayerSpawnAsync_ShouldReturnNull_WhenNoEntryPointExists()
  {
    // Arrange
    A.CallTo(() => _repository.GetPlayerSpawnAsync(A<CancellationToken>._)).Returns(Task.FromResult<Room?>(null));

    // Act
    var result = await _service.GetPlayerSpawnAsync(CancellationToken.None);

    // Assert
    Assert.Null(result);
  }

  #endregion
}
