using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing jobs.
/// </summary>
public class JobService(IJobRepository jobRepository) : IJobService
{
  #region Implementation of IJobService

  /// <inheritdoc />
  public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return await jobRepository.GetByIdAsync(id, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<Job?> GetByNameAsync(string name, CancellationToken cancellationToken)
  {
    return await jobRepository.GetByNameAsync(name, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Job>> GetAllJobsAsync(CancellationToken cancellationToken)
  {
    return await jobRepository.GetAllAsync(cancellationToken);
  }

  #endregion
}
