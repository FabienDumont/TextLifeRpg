using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
///   Service for managing narrations.
/// </summary>
public class NarrationService(INarrationRepository repository) : INarrationService
{
  #region Implementation of INarrationService

  /// <inheritdoc />
  public async Task<string> GetNarrationTextByKeyAsync(
    string key, Character actor, World world, CancellationToken cancellationToken
  )
  {
    var gameContext = new GameContext
    {
      Actor = actor,
      World = world
    };
    var narration = await repository.GetNarrationByKeyAsync(key, gameContext, cancellationToken);

    return narration.Text;
  }

  #endregion
}
