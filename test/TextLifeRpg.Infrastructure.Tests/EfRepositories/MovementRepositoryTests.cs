using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class MovementRepositoryTests
{
  #region Fields

  private readonly Guid _bedroomId = Guid.NewGuid();
  private readonly Guid _homeId = Guid.NewGuid();
  private readonly Guid _livingRoomId = Guid.NewGuid();

  private readonly MovementRepository _repository;
  private readonly Guid _streetId = Guid.NewGuid();

  #endregion

  #region Ctors

  public MovementRepositoryTests()
  {
    var locationDataModels = new List<LocationDataModel>
    {
      new() {Id = _homeId, Name = "Home"},
      new() {Id = _streetId, Name = "Street"}
    };

    var roomDataModels = new List<RoomDataModel>
    {
      new() {Id = _bedroomId, LocationId = _homeId, Name = "Bedroom", IsPlayerSpawn = true},
      new() {Id = _livingRoomId, LocationId = _homeId, Name = "Living Room", IsPlayerSpawn = false}
    };

    var movementDataModels = new List<MovementDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        FromLocationId = _homeId,
        FromRoomId = _livingRoomId,
        ToLocationId = _homeId,
        ToRoomId = _bedroomId,
        RequiredItemId = null
      },
      new()
      {
        Id = Guid.NewGuid(),
        FromLocationId = _homeId,
        FromRoomId = _livingRoomId,
        ToLocationId = _streetId,
        ToRoomId = null,
        RequiredItemId = null
      }
    };

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.Movements).Returns(movementDataModels.AsQueryable().BuildMockDbSet());
    A.CallTo(() => context.Rooms).Returns(roomDataModels.AsQueryable().BuildMockDbSet());
    A.CallTo(() => context.Locations).Returns(locationDataModels.AsQueryable().BuildMockDbSet());
    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new MovementRepository(context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetAvailableMovementsAsync_ShouldReturnMovements_InExpectedOrder()
  {
    // Act
    var result = await _repository.GetMovementsAsync(_homeId, _livingRoomId, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count);
    Assert.Equal(_homeId, result[0].ToLocationId);
    Assert.Equal(_streetId, result[1].ToLocationId);
  }

  [Fact]
  public async Task GetAvailableMovementsAsync_ShouldReturnEmptyList_WhenNoMovementsExist()
  {
    // Act
    var result = await _repository.GetMovementsAsync(Guid.NewGuid(), null, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
  }

  #endregion
}
