using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class JobServiceTests
{
  #region Fields

  private readonly IJobRepository _jobRepository = A.Fake<IJobRepository>();
  private readonly JobService _jobService;

  #endregion

  #region Ctors

  public JobServiceTests()
  {
    _jobService = new JobService(_jobRepository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetByIdAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var jobId = Guid.NewGuid();
    var job = Job.Load(jobId, string.Empty, int.MinValue, int.MinValue);
    A.CallTo(() => _jobRepository.GetByIdAsync(jobId, A<CancellationToken>._)).Returns(job);

    // Act
    var result = await _jobService.GetByIdAsync(jobId, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(jobId, result.Id);
  }

  [Fact]
  public async Task GetByNameAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var jobId = Guid.NewGuid();
    var job = Job.Load(jobId, string.Empty, int.MinValue, int.MinValue);
    A.CallTo(() => _jobRepository.GetByNameAsync(string.Empty, A<CancellationToken>._)).Returns(job);

    // Act
    var result = await _jobService.GetByNameAsync(string.Empty, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(jobId, result.Id);
  }

  [Fact]
  public async Task GetAllJobsAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var jobs = new List<Job> {Job.Load(Guid.NewGuid(), string.Empty, int.MinValue, int.MinValue)};
    A.CallTo(() => _jobRepository.GetAllAsync(A<CancellationToken>._)).Returns(jobs);

    // Act
    var result = await _jobService.GetAllJobsAsync(CancellationToken.None);
    var resultList = result.ToList();

    // Assert
    Assert.Equal(jobs.Count, resultList.Count);
    Assert.Equal(jobs[0].Id, resultList[0].Id);
  }

  #endregion
}
