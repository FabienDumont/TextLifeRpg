using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for traits.
/// </summary>
public interface IJobRepository
{
  #region Methods

  /// <summary>
  /// Retrieve a job from a given identifier.
  /// </summary>
  /// <param name="id">The job identifier.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The corresponding job.</returns>
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
  Task<IReadOnlyCollection<Job>> GetAllAsync(CancellationToken cancellationToken);

  #endregion
}
