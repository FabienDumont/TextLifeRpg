using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class ExplorationActionRepositoryTests
{
  #region Fields

  private readonly Guid _locationId = Guid.NewGuid();
  private readonly ExplorationActionRepository _repository;
  private readonly Guid _roomId = Guid.NewGuid();

  #endregion

  #region Ctors

  public ExplorationActionRepositoryTests()
  {
    var data = new List<ExplorationActionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = _locationId,
        RoomId = _roomId,
        Label = "Sleep",
        NeededMinutes = 480
      },
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = _locationId,
        RoomId = null,
        Label = "Nap",
        NeededMinutes = 60
      },
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = Guid.NewGuid(),
        RoomId = _roomId,
        Label = "Wait",
        NeededMinutes = 15
      }
    };

    var context = A.Fake<ApplicationContext>();
    var mockDbSet = data.AsQueryable().BuildMockDbSet();
    A.CallTo(() => context.ExplorationActions).Returns(mockDbSet);

    _repository = new ExplorationActionRepository(context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetByLocationAndRoomIdAsync_ShouldReturnMatchingWithRoomId()
  {
    var result = await _repository.GetByLocationAndRoomIdAsync(_locationId, _roomId, CancellationToken.None);

    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Equal(_roomId, result[0].RoomId);
    Assert.Equal(_locationId, result[0].LocationId);
  }

  [Fact]
  public async Task GetByLocationAndRoomIdAsync_ShouldReturnMatchingWithoutRoomId()
  {
    var result = await _repository.GetByLocationAndRoomIdAsync(_locationId, null, CancellationToken.None);

    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Null(result[0].RoomId);
    Assert.Equal(_locationId, result[0].LocationId);
  }

  [Fact]
  public async Task GetByLocationAndRoomIdAsync_ShouldReturnEmpty_WhenNoMatch()
  {
    var nonexistentLocation = Guid.NewGuid();

    var result = await _repository.GetByLocationAndRoomIdAsync(nonexistentLocation, _roomId, CancellationToken.None);

    Assert.NotNull(result);
    Assert.Empty(result);
  }

  #endregion
}
