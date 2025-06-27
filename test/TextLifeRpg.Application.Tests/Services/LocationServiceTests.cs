using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class LocationServiceTests
{
  #region Fields

  private readonly ILocationRepository _repository = A.Fake<ILocationRepository>();
  private readonly LocationService _service;

  #endregion

  #region Ctors

  public LocationServiceTests()
  {
    _service = new LocationService(_repository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetByIdAsync_ShouldReturnLocation_WhenLocationExists()
  {
    var expected = Location.Load(Guid.NewGuid(), "Market", []);
    A.CallTo(() => _repository.GetByIdAsync(expected.Id, A<CancellationToken>._)).Returns(Task.FromResult(expected));

    var result = await _service.GetByIdAsync(expected.Id, CancellationToken.None);

    Assert.Same(expected, result);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldThrow_WhenRepositoryThrows()
  {
    var randomId = Guid.NewGuid();
    A.CallTo(() => _repository.GetByIdAsync(randomId, A<CancellationToken>._)).Throws(
      new InvalidOperationException($"Location with ID {randomId} was not found.")
    );

    var exception =
      await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetByIdAsync(
          randomId, CancellationToken.None
        )
      );

    Assert.Equal($"Location with ID {randomId} was not found.", exception.Message);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldReturnLocation_WhenLocationExists()
  {
    var name = "Market";
    var expected = Location.Load(Guid.NewGuid(), name, []);
    A.CallTo(() => _repository.GetByNameAsync(name, A<CancellationToken>._))
      .Returns(Task.FromResult<Location?>(expected));

    var result = await _service.GetByNameAsync(name, CancellationToken.None);

    Assert.Same(expected, result);
  }

  [Fact]
  public async Task IsLocationOpenAsync_ShouldReturnTrue_WhenLocationIsAlwaysOpen()
  {
    var locationId = Guid.NewGuid();
    var location = Location.Load(locationId, "Library", []);
    var time = new TimeSpan(10, 0, 0);
    const DayOfWeek day = DayOfWeek.Tuesday;

    A.CallTo(() => _repository.GetByIdAsync(locationId, A<CancellationToken>._)).Returns(location);

    var result = await _service.IsLocationOpenAsync(locationId, day, time, CancellationToken.None);

    Assert.True(result);
  }

  [Fact]
  public async Task IsLocationOpenAsync_ShouldDelegateToOpeningHoursService_WhenLocationIsNotAlwaysOpen()
  {
    var locationId = Guid.NewGuid();
    const DayOfWeek day = DayOfWeek.Monday;
    var openingTime = new TimeSpan(14, 0, 0);
    var closingTime = new TimeSpan(16, 0, 0);
    var location = Location.Load(
      locationId, "College", [LocationOpeningHours.Create(locationId, day, openingTime, closingTime)]
    );

    A.CallTo(() => _repository.GetByIdAsync(locationId, A<CancellationToken>._)).Returns(location);

    var result = await _service.IsLocationOpenAsync(locationId, day, openingTime, CancellationToken.None);

    Assert.True(result);
  }

  #endregion
}
