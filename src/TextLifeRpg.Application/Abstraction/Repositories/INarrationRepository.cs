using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
///   Repository interface for narrations.
/// </summary>
public interface INarrationRepository
{
  #region Methods

  /// <summary>
  ///   Retrieves the narration associated with a specific key for the specified game context.
  /// </summary>
  Task<Narration> GetNarrationByKeyAsync(string key, GameContext gameContext, CancellationToken cancellationToken);

  #endregion
}
