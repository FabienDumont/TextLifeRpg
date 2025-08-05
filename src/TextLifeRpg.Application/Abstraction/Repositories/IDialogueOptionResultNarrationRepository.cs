using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for dialogue option result narrations.
/// </summary>
public interface IDialogueOptionResultNarrationRepository
{
  #region Methods

  /// <summary>
  /// Retrieves a narration associated with the specified dialogue option result ID.
  /// </summary>
  /// <param name="dialogueOptionResultId">The unique identifier of the dialogue option result.</param>
  /// <param name="gameContext">The current game context including the actor, world state, and optional target.</param>
  /// <param name="cancellationToken">Cancellation token to cancel the asynchronous operation if needed.</param>
  /// <returns>A task representing the asynchronous operation. The task result contains the narration as a string.</returns>
  Task<string> GetByDialogueOptionResultIdAsync(
    Guid dialogueOptionResultId, GameContext gameContext, CancellationToken cancellationToken
  );

  #endregion
}
