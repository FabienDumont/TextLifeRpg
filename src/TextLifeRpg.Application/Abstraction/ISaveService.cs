using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing saves.
/// </summary>
public interface ISaveService
{
  #region Methods

  /// <summary>
  /// Persists a game save asynchronously.
  /// </summary>
  /// <param name="save">The game save to persist.</param>
  /// <param name="ct">A cancellation token.</param>
  Task SaveGameAsync(GameSave save, CancellationToken ct = default);

  /// <summary>
  /// Loads a game save from its JSON representation.
  /// </summary>
  /// <param name="json">The JSON string representing the save data.</param>
  /// <returns>The loaded game save, or null if deserialization fails.</returns>
  GameSave? LoadGame(string json);

  /// <summary>
  /// Creates a new game save instance.
  /// </summary>
  /// <param name="date">The current in-game date.</param>
  /// <param name="playerCharacter">The player's character.</param>
  /// <param name="gameSettings">The game settings to use.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A new game save instance.</returns>
  Task<GameSave> CreateNewSaveAsync(
    DateTime date, Character playerCharacter, GameSettings gameSettings, CancellationToken cancellationToken
  );

  #endregion
}
