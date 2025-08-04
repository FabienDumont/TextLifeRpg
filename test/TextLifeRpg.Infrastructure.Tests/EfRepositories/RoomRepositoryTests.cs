using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class RoomRepositoryTests
{
  #region Fields

  private readonly ApplicationContext _context;
  private readonly Guid _locationId = Guid.NewGuid();
  private readonly RoomRepository _repository;
  private readonly List<RoomDataModel> _roomDataModels;

  #endregion

  #region Ctors

  public RoomRepositoryTests()
  {
    _context = A.Fake<ApplicationContext>();

    _roomDataModels =
    [
      new RoomDataModel {Id = Guid.NewGuid(), LocationId = _locationId, Name = "Bedroom", IsPlayerSpawn = false},
      new RoomDataModel {Id = Guid.NewGuid(), LocationId = _locationId, Name = "Living room", IsPlayerSpawn = true}
    ];

    var roomDbSet = _roomDataModels.BuildMockDbSet();

    A.CallTo(() => _context.Rooms).Returns(roomDbSet);
    A.CallTo(() => _context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new RoomRepository(_context);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetLocationEntryPointAsync_ShouldReturnPlayerSpawn()
  {
    var result = await _repository.GetPlayerSpawnAsync(CancellationToken.None);
    Assert.NotNull(result);
    Assert.True(result.IsPlayerSpawn);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldReturnRoom_WhenRoomExists()
  {
    var roomId = _roomDataModels.First().Id;

    A.CallTo(() => _context.Rooms.FindAsync(
        A<object[]>.That.Matches(keys => (Guid) keys[0] == roomId), A<CancellationToken>._
      )
    ).Returns(new ValueTask<RoomDataModel?>(_roomDataModels.First(r => r.Id == roomId)));

    var result = await _repository.GetByIdAsync(roomId, CancellationToken.None);

    Assert.NotNull(result);
    Assert.Equal(roomId, result.Id);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldThrow_WhenRoomDoesNotExist()
  {
    var randomId = Guid.NewGuid();

    A.CallTo(() => _context.Rooms.FindAsync(
        A<object[]>.That.Matches(keys => (Guid) keys[0] == randomId), A<CancellationToken>._
      )
    ).Returns(new ValueTask<RoomDataModel?>((RoomDataModel?) null));

    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      _repository.GetByIdAsync(randomId, CancellationToken.None)
    );

    Assert.Equal($"Room with ID {randomId} was not found.", ex.Message);
  }

  #endregion
}
