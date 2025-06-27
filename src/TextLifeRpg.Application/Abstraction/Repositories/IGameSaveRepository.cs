using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for game saves.
/// </summary>
public interface IGameSaveRepository
{
  #region Methods

  /// <summary>
  /// Persists a game save to storage.
  /// </summary>
  Task SaveAsync(GameSave save, CancellationToken cancellationToken);

  /// <summary>
  /// Loads a game save from a JSON string.
  /// </summary>
  GameSave? Load(string json);

  #endregion
}
