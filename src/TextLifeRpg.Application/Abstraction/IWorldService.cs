using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing worlds.
/// </summary>
public interface IWorldService
{
  #region Methods

  /// <summary>
  /// Creates a new world instance with the specified date, player character, and game settings.
  /// </summary>
  /// <param name="date">The initial date of the world.</param>
  /// <param name="playerCharacter">The player character to include in the world.</param>
  /// <param name="gameSettings">The game settings used to configure the world.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The created world instance.</returns>
  Task<World> CreateNewWorldAsync(
    DateTime date, Character playerCharacter, GameSettings gameSettings, CancellationToken cancellationToken
  );

  /// <summary>
  /// Advances the in-game time by a specified number of minutes.
  /// </summary>
  /// <param name="world">The world instance to update.</param>
  /// <param name="playerCharacterId">The identifier of the player character.</param>
  /// <param name="minutes">The number of minutes to advance.</param>
  void AdvanceTime(World world, Guid playerCharacterId, int minutes);

  #endregion
}
