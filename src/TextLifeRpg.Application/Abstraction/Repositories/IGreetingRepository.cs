using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for greetings.
/// </summary>
public interface IGreetingRepository
{
  #region Methods

  /// <summary>
  /// Retrieves a greeting depending on the given game context.
  /// </summary>
  Task<IReadOnlyList<string>> GetAsync(GameContext gameContext, CancellationToken cancellationToken);

  #endregion
}
