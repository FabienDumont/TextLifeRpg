using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for dialogue option result spoken texts.
/// </summary>
public interface IDialogueOptionResultSpokenTextRepository
{
  #region Methods

  /// <summary>
  /// Retrieves a list of spoken text associated with a specific dialogue option result.
  /// </summary>
  /// <param name="dialogueOptionResultId">
  /// The unique identifier of the dialogue option result for which the spoken text is retrieved.
  /// </param>
  /// <param name="gameContext">
  /// The context in which the game is being played, providing relevant information such as the actor and world.
  /// </param>
  /// <param name="cancellationToken">
  /// The token used to cancel the operation, if necessary, to manage task cancellation.
  /// </param>
  /// <returns>
  /// A task representing the asynchronous operation, containing a read-only list of strings that are the spoken text associated with the dialogue option result.
  /// </returns>
  Task<IReadOnlyList<string>> GetByDialogueOptionResultIdAsync(
    Guid dialogueOptionResultId, GameContext gameContext, CancellationToken cancellationToken
  );

  #endregion
}
