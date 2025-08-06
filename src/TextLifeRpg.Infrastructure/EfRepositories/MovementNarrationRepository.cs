using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for movement narrations.
/// </summary>
public class MovementNarrationRepository(ApplicationContext context)
  : RepositoryBase(context), IMovementNarrationRepository
{
  #region Implementation of IMovementNarrationRepository

  /// <inheritdoc />
  public async Task<string> GetMovementNarrationFromMovementIdAsync(
    Guid movementId, CancellationToken cancellationToken
  )
  {
    var dataModel = await Context.MovementNarrations.FirstOrDefaultAsync(
      n => n.MovementId == movementId, cancellationToken
    );

    if (dataModel is null)
    {
      throw new InvalidOperationException($"No narration found for movement {movementId}");
    }

    return dataModel.Text;
  }

  #endregion
}
