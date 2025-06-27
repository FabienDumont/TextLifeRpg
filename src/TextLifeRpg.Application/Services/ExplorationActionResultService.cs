using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
///   Service for exploration action results.
/// </summary>
public class ExplorationActionResultService(IExplorationActionResultRepository explorationActionResultRepository)
  : IExplorationActionResultService
{
  #region Implementation of IExplorationActionResultService

  /// <inheritdoc />
  public async Task<ExplorationActionResult> GetExplorationActionResultAsync(
    Guid explorationActionId, Character actor, World world, CancellationToken cancellationToken
  )
  {
    var gameContext = new GameContext
    {
      Actor = actor,
      World = world
    };

    var explorationActionResult =
      await explorationActionResultRepository.GetByExplorationActionIdAsync(
        explorationActionId, gameContext, cancellationToken
      );

    return explorationActionResult;
  }

  #endregion
}
