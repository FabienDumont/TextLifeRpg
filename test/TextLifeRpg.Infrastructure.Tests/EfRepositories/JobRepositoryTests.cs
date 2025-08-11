using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class JobRepositoryTests
{
  #region Fields

  private readonly List<JobDataModel> _jobs;
  private readonly JobRepository _repository;

  #endregion

  #region Ctors

  public JobRepositoryTests()
  {
    var context = A.Fake<ApplicationContext>();

    _jobs =
    [
      new JobDataModel {Id = Guid.NewGuid(), Name = "Cashier", HourIncome = int.MinValue, MaxWorkers = int.MinValue},
      new JobDataModel {Id = Guid.NewGuid(), Name = "Mechanic", HourIncome = int.MinValue, MaxWorkers = int.MinValue}
    ];

    var mockDbSet = _jobs.BuildMockDbSet();
    A.CallTo(() => context.Jobs).Returns(mockDbSet);

    _repository = new JobRepository(context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetById_ShouldReturnMappedJob_WhenItExists()
  {
    // Arrange
    var existing = _jobs[1];

    // Act
    var result = await _repository.GetByIdAsync(existing.Id, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(existing.Id, result.Id);
    Assert.Equal(existing.Name, result.Name);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldReturnMappedJob_WhenExists()
  {
    // Arrange
    var target = _jobs[1];

    // Act
    var result = await _repository.GetByNameAsync(target.Name, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(target.Id, result.Id);
    Assert.Equal(target.Name, result.Name);
    Assert.Equal(target.HourIncome, result.HourIncome);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldReturnNull_WhenNotFound()
  {
    // Act
    var result = await _repository.GetByNameAsync("Nothing", CancellationToken.None);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnMappedJobs()
  {
    var result = await _repository.GetAllAsync(CancellationToken.None);

    Assert.NotNull(result);
    Assert.Equal(_jobs.Count, result.Count);
    foreach (var job in _jobs)
    {
      Assert.Contains(result, r => r.Id == job.Id);
    }
  }

  #endregion
}
