namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Base class for EF Core repositories, providing access to the database context.
/// </summary>
public abstract class RepositoryBase(ApplicationContext context)
{
  #region Properties

  /// <summary>
  /// The database context used by the repository.
  /// </summary>
  protected ApplicationContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));

  #endregion

  #region Methods

  /// <summary>
  /// Persists changes made in the context to the database.
  /// </summary>
  public Task SaveAsync(CancellationToken cancellationToken)
  {
    return Context.SaveChangesAsync(cancellationToken);
  }

  #endregion
}
