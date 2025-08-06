using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for dialogue option spoken texts.
/// </summary>
public interface IDialogueOptionSpokenTextRepository
{
  #region Methods

  /// <summary>
  /// Retrieves the spoken text associated with a specific dialogue option ID within the given game context.
  /// </summary>
  /// <param name="dialogueOptionId">The unique identifier of the dialogue option.</param>
  /// <param name="gameContext">The current state of the game, including the actor and world context.</param>
  /// <param name="cancellationToken">Token to signal the operation should be canceled.</param>
  /// <returns>A task representing the asynchronous operation, which upon completion contains the spoken text.</returns>
  Task<string> GetByDialogueOptionIdAsync(
    Guid dialogueOptionId, GameContext gameContext, CancellationToken cancellationToken
  );

  #endregion
}
