using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing jobs.
/// </summary>
public interface IJobService
{
  #region Methods

  /// <summary>
  /// Retrieve a job from a given identifier.
  /// </summary>
  /// <param name="id">The job identifier.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The corresponding trait.</returns>
  Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieve a job from a given name.
  /// </summary>
  /// <param name="name">The job name.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The corresponding trait.</returns>
  Task<Job?> GetByNameAsync(string name, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves all available jobs.
  /// </summary>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A read-only collection of jobs.</returns>
  Task<IReadOnlyCollection<Job>> GetAllJobsAsync(CancellationToken cancellationToken);

  #endregion
}
