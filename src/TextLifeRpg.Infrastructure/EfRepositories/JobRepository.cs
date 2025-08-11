using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for jobs.
/// </summary>
public class JobRepository(ApplicationContext context) : RepositoryBase(context), IJobRepository
{
  #region Implementation of IJobRepository

  /// <inheritdoc />
  public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var job = await Context.Jobs.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    return job?.ToDomain();
  }

  /// <inheritdoc />
  public async Task<Job?> GetByNameAsync(string name, CancellationToken cancellationToken)
  {
    var job = await Context.Jobs.FirstOrDefaultAsync(t => t.Name == name, cancellationToken);

    return job?.ToDomain();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Job>> GetAllAsync(CancellationToken cancellationToken)
  {
    var dataModels = await Context.Jobs.ToListAsync(cancellationToken).ConfigureAwait(false);

    return dataModels.ToDomainCollection();
  }

  #endregion
}
