﻿using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for movement narrations.
/// </summary>
public class MovementNarrationRepository(ApplicationContext context)
  : RepositoryBase(context), IMovementNarrationRepository
{
  #region Implementation of IMovementNarrationRepository

  /// <inheritdoc />
  public async Task<MovementNarration> GetMovementNarrationFromMovementIdAsync(
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

    return dataModel.ToDomain();
  }

  #endregion
}
