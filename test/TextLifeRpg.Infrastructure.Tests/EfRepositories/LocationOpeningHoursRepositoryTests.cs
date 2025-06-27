using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class LocationOpeningHoursRepositoryTests
{
  #region Fields

  private readonly Guid _collegeId = Guid.NewGuid();
  private readonly LocationOpeningHoursRepository _repository;

  #endregion

  #region Ctors

  public LocationOpeningHoursRepositoryTests()
  {
    var openingHoursData = new List<LocationOpeningHoursDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = _collegeId,
        DayOfWeek = DayOfWeek.Monday,
        OpensAt = new TimeSpan(8, 0, 0),
        ClosesAt = new TimeSpan(18, 0, 0)
      },
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = _collegeId,
        DayOfWeek = DayOfWeek.Tuesday,
        OpensAt = new TimeSpan(8, 0, 0),
        ClosesAt = new TimeSpan(18, 0, 0)
      }
    };

    var context = A.Fake<ApplicationContext>();
    var mockDbSet = openingHoursData.AsQueryable().BuildMockDbSet();

    A.CallTo(() => context.LocationOpeningHours).Returns(mockDbSet);
    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new LocationOpeningHoursRepository(context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetByLocationIdAsync_ShouldReturnMatchingOpeningHours()
  {
    // Act
    var result = await _repository.GetByLocationIdAsync(_collegeId, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count);
    Assert.All(result, oh => Assert.Equal(_collegeId, oh.LocationId));
  }

  [Fact]
  public async Task GetByLocationIdAsync_ShouldReturnEmptyList_WhenNoMatchingLocation()
  {
    // Act
    var result = await _repository.GetByLocationIdAsync(Guid.NewGuid(), CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
  }

  #endregion
}
