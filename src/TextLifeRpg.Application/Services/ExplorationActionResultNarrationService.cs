using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for exploration action result narrations.
/// </summary>
public class ExplorationActionResultNarrationService(
  IExplorationActionResultNarrationRepository explorationActionResultNarrationRepository
) : IExplorationActionResultNarrationService
{
  #region Implementation of IExplorationActionResultNarrationService

  /// <inheritdoc />
  public async Task<ExplorationActionResultNarration> GetExplorationActionResultNarrationAsync(
    Guid explorationActionResultId, Character actor, World world, CancellationToken cancellationToken
  )
  {
    var gameContext = new GameContext
    {
      Actor = actor,
      World = world
    };

    var explorationActionResultNarration =
      await explorationActionResultNarrationRepository.GetByExplorationActionResultIdAsync(
        explorationActionResultId, gameContext, cancellationToken
      );

    return explorationActionResultNarration;
  }

  #endregion
}
