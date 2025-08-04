using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class LocationRepositoryTests
{
  #region Fields

  private readonly ApplicationContext _context;
  private readonly List<LocationDataModel> _locationDataModels;
  private readonly LocationRepository _repository;

  #endregion

  #region Ctors

  public LocationRepositoryTests()
  {
    _locationDataModels =
    [
      new LocationDataModel {Id = Guid.NewGuid(), Name = "Home"},
      new LocationDataModel {Id = Guid.NewGuid(), Name = "Street"}
    ];

    _context = A.Fake<ApplicationContext>();
    var locationsDbSet = _locationDataModels.BuildMockDbSet();

    A.CallTo(() => _context.Locations).Returns(locationsDbSet);
    A.CallTo(() => _context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new LocationRepository(_context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetByIdAsync_ShouldReturnLocation_WhenLocationExists()
  {
    // Arrange
    var existingLocation = _locationDataModels.First();
    var fakeContext = A.Fake<ApplicationContext>();
    var locationsDbSet = _locationDataModels.BuildMockDbSet();

    A.CallTo(() => fakeContext.Locations).Returns(locationsDbSet);
    A.CallTo(() => fakeContext.Locations.FindAsync(
        A<object[]>.That.Matches(keys => (Guid) keys[0] == existingLocation.Id), A<CancellationToken>._
      )
    ).Returns(new ValueTask<LocationDataModel?>(existingLocation));

    var repository = new LocationRepository(fakeContext);

    // Act
    var result = await repository.GetByIdAsync(existingLocation.Id, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(existingLocation.Id, result.Id);
    Assert.Equal(existingLocation.Name, result.Name);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldThrowInvalidOperationException_WhenLocationDoesNotExist()
  {
    // Arrange
    var randomId = Guid.NewGuid();

    A.CallTo(() => _context.Locations.FindAsync(A<object[]>.Ignored, A<CancellationToken>._))
      .Returns(new ValueTask<LocationDataModel?>((LocationDataModel?) null));

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _repository.GetByIdAsync(randomId, CancellationToken.None)
    );

    Assert.Equal($"Location with ID {randomId} was not found.", ex.Message);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldReturnLocation_WhenLocationExists()
  {
    // Arrange
    var existingLocation = _locationDataModels.First();
    var fakeContext = A.Fake<ApplicationContext>();
    var locationsDbSet = _locationDataModels.BuildMockDbSet();

    A.CallTo(() => fakeContext.Locations).Returns(locationsDbSet);
    A.CallTo(() => fakeContext.Locations.FindAsync(
        A<object[]>.That.Matches(keys => (Guid) keys[0] == existingLocation.Id), A<CancellationToken>._
      )
    ).Returns(new ValueTask<LocationDataModel?>(existingLocation));

    var repository = new LocationRepository(fakeContext);

    // Act
    var result = await repository.GetByNameAsync(existingLocation.Name, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(existingLocation.Id, result.Id);
    Assert.Equal(existingLocation.Name, result.Name);
  }

  #endregion
}
